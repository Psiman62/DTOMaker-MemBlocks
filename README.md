# DTOMaker.MemBlocks

todo badges go here

*Warning: This is pre-release software under active development. Not for production use. Do not use if you can't tolerate breaking changes occasionally.*

Model-driven source generator for data transport objects (DTOs).

Generates DTOs whose internal data is a single memory block (Memory\<byte\>). Property getters and setters decode and encode
values directly to the block with explicit byte ordering (little-endian or big-endian).
