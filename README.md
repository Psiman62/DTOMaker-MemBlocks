# DTOMaker.MemBlocks

todo badges go here

*Warning: This is pre-release software under active development. Not for production use. Do not use if you can't tolerate breaking changes occasionally.*

Model-driven source generator for data transport objects (DTOs).

Generates DTOs whose internal data is a single memory block (Memory\<byte\>). Property getters and setters decode and encode
values directly to the block with explicit byte ordering (little-endian or big-endian).

## Quick Start
To use this generator:
1. create a new C# library project to contain your DTOs;
2. add a package reference to DTOMaker.Core;
3. define your models in C# as interfaces with simple properties;
4. add a reference to this package;
5. build your project;
6. voila! your generated MessagePack DTOs are ready to use.
