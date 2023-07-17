# Memory!
Mistake. The initial setup of memory and programs is done in the 6502. This is obviously wrong.
The processor has access to up to 16bts of memoory. It doesn't actually own it.

1. Create a memory array
2. Prep it with data.

ALl programs start with the reset handler.
Value of the reset vector at FFFC/FFFD is translated as memory[value]
That is the start position of your program.

Want to load a new program?
Update the reset vector at FC/FD update the memory, call reset.
There is no RTI for a reset.
That's how you read code.

"All of the vectors used for operating system routines
are initialised on reset by the operating system. Normally each
vector contains the address of the relevant routine in the
operating system."

 A little confused about how we are meabt to be using this cpu.
 It's just a cpu. There are 56 instructions it can accomplish.
 It has very little affect outside.
 The vectors are an example of this. They are just addresses. Everything else is up to the implementer.

 It has a access to meory