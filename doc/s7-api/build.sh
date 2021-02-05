#!/bin/sh
mkdir -p config/
mkdir -p output/
naturaldocs2 -r -nag -i src/ -p config/ -o html output/
