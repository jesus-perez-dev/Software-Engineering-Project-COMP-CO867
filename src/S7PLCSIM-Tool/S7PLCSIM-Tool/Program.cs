using System;
using System.Linq;
using System.Text;
using S7PLCSIM_Library;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Tool
{
    
    /* Features:
     * - Create, start, stop, and reset simulation instances
     * - Create and list address (input and output)
     * - Read values from output addresses
     * - Write values to input addresses
     */
    
    class Program
    {
        static readonly string HelpText = string.Join("\n", new string[]
        {
          "Commands:",
          "\tcreate $name $location $type - Create input/output address called $name at simulation $address (QX.X or IX.X) using primitive type $type (See: Address Types)",
          "\tlist - List the simulation instance and input/output addresses created with the tool",
          "\tread $name - Read value from the simulation at the previously created output address $name",
          "\twrite $name $value  - Write an integer value to the simulation at the previously created input address $name",
          "\tdump $address $bytes - Read and dump simulator memory address range from $address to $address+$bytes",
          "\trun - Change simulation to RUN operating state",
          "\tstop - Change simulation to STOP operating state",
          "\tpoweron - Power On the simulation",
          "\tpoweroff - Power Off the simulation",
          "\treset - Reset simulation memory",
          "\tquit - Quit the tool",
          "\thelp - Show this help",
          "",
          "Examples:",
          "\tcreate example_input %I2.2 UInt8",
          "\tread example_input",
          "\twrite example_input 128",
          "\tdump %Q1.0 5",
          "",
          "Address Types:",
          "\tBool, Int8, Int16, Int32, Int64, UInt8, UInt16, UInt32, UInt64, Float, Double, Char, WChar"
        } );

        public static SimulationClient Setup()
        {
            string? name = null;
            do
            {
                Console.Write("S7-PLCSIM Instance Name: ");
                Console.Out.Flush();
                name = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(name));
            
            return new SimulationClient(name);
        }

        public static void Loop(SimulationClient client)
        {
            Console.WriteLine(@"Type ""help"" for usage instructions");
            bool quit = false;
            while (!quit)
            {
                Console.Write("> ");
                Console.Out.Flush();
                string? line = Console.ReadLine();

                if (line == null)
                    break;
                
                string[] tokens = line.Split();

                try
                {
                    switch (tokens[0])
                    {
                        case "create":
                            if (tokens.Length == 4)
                                CommandCreate(client, tokens[1], tokens[2], tokens[3]);
                            else
                                Console.Error.WriteLine("`create` command requires 3 arguments");
                            break;
                        case "list":
                            CommandList(client);
                            break;
                        case "dump":
                            if (tokens.Length == 3)
                                CommandDump(client, tokens[1], tokens[2]);
                            else
                                Console.Error.WriteLine("`dump` command requires 2 arguments");
                            break;
                        case "read":
                            if (tokens.Length == 2)
                                CommandRead(client, tokens[1]);
                            else
                                Console.Error.WriteLine("`read` command requires 1 arguments");
                            break;
                        case "write":
                            if (tokens.Length == 3)
                                CommandWrite(client, tokens[1], tokens[2]);
                            else
                                Console.Error.WriteLine("`write` command requires 2arguments");
                            break;
                        case "poweron":
                            client.PowerOn();
                            break;
                        case "poweroff":
                            client.PowerOff();
                            break;
                        case "run":
                            client.Run();
                            break;
                        case "stop":
                            client.Stop();
                            break;
                        case "reset":
                            client.MemoryReset();
                            break;
                        case "help":
                            Console.WriteLine(HelpText);
                            break;
                        case "quit":
                            quit = true;
                            break;
                        default:
                            Console.Error.WriteLine($"Invalid Command: {tokens[0]}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }

        static void Main(string[] args)
        {
            SimulationClient client = Setup();
            Loop(client);
        }

        private static void CommandList(SimulationClient client)
        {
            Console.WriteLine($"Simulation Client Status");
            Console.WriteLine($"\tName - {client.Instance.Info.Name}");
            Console.WriteLine($"\tInfo - {client.Instance.Info.ToString()}");
            Console.WriteLine($"\tOperating Mode - {Enum.GetName(typeof(EOperatingMode), client.Instance.OperatingMode)}");
            Console.WriteLine($"\tOperating State - {Enum.GetName(typeof(EOperatingState), client.Instance.OperatingState)}");
            
            Console.WriteLine($"Input (%I) Addresses ({client.IAddress.Count})");
            foreach (var (name, value) in client.IAddress) 
            {
                Console.WriteLine($"\tName: {name}");
                Console.WriteLine($"\tAddress: {value}");
                Console.WriteLine($"\tValue: \n\t\t{value.Value.ToStringAll("\n\t\t")}");
            }
            
            Console.WriteLine($"Output (%Q) Addresses ({client.QAddress.Count})");
            foreach (var (name, value) in client.QAddress)
            {
                Console.WriteLine($"\tName: {name}");
                Console.WriteLine($"\tAddress: {value}");
                Console.WriteLine($"\tValue: \n\t\t{value.Value.ToStringAll("\n\t\t")}");
            }
        }

        private static (IIOArea?, uint, byte, char) ParseAddress(SimulationClient client, string address)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(address, "%[QI]\\d+\\.\\d+"))
            {
                Console.Error.WriteLine("Address format Error: Address must match '%Qx.x' or 'Ix.x' where 'x' are integers.");
                return (null, 0, (byte)0, '\0');
            }

            char addressCharacter = address[1]; // Either 'Q' or 'I'
            
            IIOArea area;
            if (addressCharacter == 'Q')
                area = client.Instance.OutputArea;
            else if (addressCharacter == 'I')
                area = client.Instance.InputArea;
            else
                throw new ArgumentException($"Invalid Address: {address}");

            string[] toks = address.Substring(2).Split('.');
            uint addressByte = uint.Parse(toks[0]);
            byte addressBit = byte.Parse(toks[1]);
            
            return (area, addressByte, addressBit, addressCharacter);
        }

        private static void CommandDump(SimulationClient client, string address, string bytesString)
        {
            var (area, addressByte, _, _) = ParseAddress(client, address);

            if (area == null)
            {
                return;
            }
            
            bool bytesParsed = uint.TryParse(bytesString, out uint bytes);
            if (!bytesParsed)
            {
                Console.Error.WriteLine($"Cannot parse \"{bytesString}\" as unsigned integer");
                return;
            }
            
            // TODO: Do we actually want to use a the bit value?
            byte[] buffer = area.ReadBytes(addressByte, bytes);
            Console.WriteLine(FormatBytes(buffer, addressByte));
        }

        private static string FormatBytes(byte[] buffer, uint offset)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Byte Array Format");
            sb.AppendLine("\t[" + string.Join(",", buffer) + "]");
            sb.AppendLine();
            sb.AppendLine("Binary Format");
            for (int i=0; i < buffer.Length; i++)
            {
                sb.AppendLine($"\t%{i}+{offset}: " + Convert.ToString(buffer[i], 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        private static void CommandRead(SimulationClient client, string name)
        {
            Console.WriteLine($"Simulation Value of \"{name}\" / {client.QAddress[name]}" );
            Console.WriteLine("\t" + client.QAddress[name].Read().ToStringAll("\n\t"));
        }
        
        private static void CommandWrite(SimulationClient client, string name, string value)
        {
            // TODO: How do we set appropriate SDataValue fields based on address value type?
            // Should we have a generic extension method somewhere?
            client.IAddress[name].Write(new SDataValue() { UInt64 = ulong.Parse(value)});
        }

        private static void CommandCreate(SimulationClient client, string name, string location, string type)
        {
            var (_, addressByte, addressBit, addressCharacter) = ParseAddress(client, location);
            bool typeParseSuccess = Enum.TryParse(type, out EPrimitiveDataType dataType);
            if (!typeParseSuccess)
            {
                Console.Error.WriteLine($"Error parsing EPrimitiveDataType data type from \"{type}\" - Try one of the following: Int32, Bool, Uint8 ...");
                return;
            }

            if (addressCharacter == 'Q')
            {
                client.QAddress.Add(name, addressByte, addressBit, dataType);
            }
            else if (addressCharacter == 'I')
            {
                client.IAddress.Add(name, addressByte, addressBit, dataType);
            }
            else
            {
                Console.Error.WriteLine($"Invalid address: {location}");
                return;
            }
        }
    }
}