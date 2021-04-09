# Mohawk S7-PLCSIM Helper Library

## Overview

The Mohawk S7-PLCSIM Helper Library extends the Siemens C# Simulation API library
(namespace `Siemens.Simatic.Simulation.Runtime`) to provide an interface for
registering "regions" of simulation memory where IO operations can be executed.

*Regions* of simulation memory start at an address specified by a *byte offset*
and a *bit offset*, and span according to a *bit size* value.

Memory regions in `%I` memory are represented by `SimulationInput` address
objects, and `SimulationOutput` objects represent %Q memory regions.
Both object types inherit from a common abstract class called `SimulationAddress`.

The main library class `SimulationClient` contains 2 key-value (C# Dictionary
compatible) collections of type `SimulationAddressContainer` representing
memory IO regions identified by a "name". These "name" values have no logical
impact on IO operations, they exist to help library users distinguish between
memory regions in a programmer-friendly way and to help document intended
functionality of the address.

## Class Descriptions

The library is comprised of 4 classes:

**SimulationClient**: Main class of the library. Provides management facilities to the PLC simulation instance (create, register, poweron, poweroff, etc) and maintains collections of `SimulationAddress` objects for simulation memory I/O.

**SimulationAddress**: Abstract class that represents a region of memory. Cannot be used directly, use one of the subclasses described below:

  - **SimulationInput**: Represents a region of memory in "%I" address space. Contains a `Write()` method for setting values in simulation memory.

  - **SimulationOutput**: Represents a region of memory in "%Q" address space. Contains a `Read()` method for retrieving values from simulation memory.

**SimulationAddressContainer**: Represents a collection of `SimulationInput` or `SimulationOutput` objects. `Add()` and `Remove()` methods instantiate `SimulationAddress` objects which can be accessed using C# dictionary lookup `[]` operator syntax.

## Memory Regions

Memory regions represent a single scalar value (ie. a C# primitive type, bool,
int, long, etc) that can occupy anywhere between 1 bit to 64 bits of simulation
memory. Region geometries are defined by `ByteOffet`, `BitOffset`, and `BitSize` values.

`ByteOffset` and `BitOffset` are equivilent to values used in Siemens PLC addressing syntax, eg. `%Q[ByteOffset].[BitOffset]`

`BitSize` defines how many bits after the starting bit the memory region will span for.

### Example Scenario

Suppose we want to reserve a region of simulation memory starting from "%Q2.3" that
occupies 4 bits. Assume this 4-bit integer value controls the speed of a motor.

Our offset and size values would look like this:

`ByteOffset = 2, BitOffset = 3, BitSize = 4`

To register this memory region with the library, we would initialize a new
simulation instance and add a memory region named "motor-speed" and specify the
values:

#### Example Code

<pre>
// "example-simulation" will appear in PLCSIM Advanced V3.0 Control Panel
SimulationClient client = new SimulationClient("example-simulation");

// PowerOn and Run the simulation
client.PowerOn();
client.Run();

// Reserve a PLC memory region in %Q address space to contain the motor speed value.
// This creates a new 'SimulationOutput' object referenceed by string "motor-speed"
client.QAddress.Add("motor-speed", 2, 3, EPrimitiveDatatype.Byte, 4);

// The simulation output object can be retrieved using C# Dictioanry accessor ([]) operator
SimulationOutput outputAddress = client.QAddress["motor-speed"];

// Read the union struct containing the bits from QAddress (up to BitSize = 4)
SDataValue motorSpeedData = outputAddress.Read();

// Print the 4 bits of memory as a C# byte value
Console.WriteLine(motorSpeedData.UInt8);

// The equivalent boolean value represented by the 4 bits can also be extracted (would print 'true' if the least-significant bit is high)
// This works because 'SDataValue' is a "union" struct type - All of its properties overlap the same 64-bit range
Console.WriteLine(motorSpeedData.Bool);
</pre>

This code creates a `SimulationOutput` object occupying 4 bits of memory
starting from "%I2.3" spanning to "%I2.6", inclusive. If a PLC program block
were to write values to this address, it would be reflected by the result of
`outputAddress.Read()` at the time it is called.

Once a memory region has been `Add()`ed, the library user must not register
another address that "overlaps" it or the library will raise an exception.
For example, adding another input address defined with geometry values
`ByteOffset=2`, `BitOffset=0` and `BitSize=8` would be invalid because this address
occupies portions of memory that "overlap" the memory range from the previous
example. This applies to partial overlaps as well.

*Note*: Memory regions can also span byte boundaries. For example, it is perfectly
valid for a region to span 5 bits from "%I2.6" to "%I3.2". The library handles
appropriately reading/writing only bits that fit within the specified memory
address region.
