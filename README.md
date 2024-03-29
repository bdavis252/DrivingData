# DrivingData
Eats files and produces reports

Initial Questions:
 1. Is the input data always clean? like can there be rows of garbage input that need to be accounted for in the file reading parser

OK so after reading it, I decided to implement a C# console project since that would be really simple to execute repeatedly (speeding debugging) as well as package up to run on your machines. Trying to think win-win. 

As far as my initial question, I reread the problem statement and noticed "Each line in the input file will start with a command," and "If the problem statement doesn't specify something, you can make any decision that you want" in the initial instructions. I'd love to assume that whatever is generating the "input file" would have error handling in it and only output one of the acceptable commands, but my experience is screaming at me not to. I just don't think that would be safe enough so I'm going to at least put some rudimentary handling to not crash and tell the user it found a bad line. And that may mean that it starts with a command but the data is still junk.

I'm going to start with the report. I'm going to make the data structure and hardcode some input and feed it to my report making thing as my first test. Then I'll make two text files, one with a single driver command and one with a single trip command. I'll then make the parser and a second test to ensure the input gets in. A third test to connect the two, and then as many test cases as I think are practical and necessary to test something more real-world. 

Also, I'm thinking of architecting this thing in the way I am because, while this is an exercise, in real life, you really are going to have some service that "registers drivers" and then a service that feeds trip data by driver. The report would pull from a database instead of text file but in the end I'm still going to end up with a service that consumes a list of trip data and produces a report. So I'm going to structure it more like a web API even though I'm writing it as a console app for convenience. 

Not going to be sadistic and output a finely-formatted PDF ... Probably.

So far the program has gone relatively smoothly. As I refactored the app to have multiple services, I had to decide whether to unit test each method or just the public-facing ones. In the end I decided that there wasn't much use testing a method that effectively just puts something on a list and so just did the public-facing ones. I also see that to filter the miles per hour I need a little refactor, because I was previously calculating mph on the Trip Summary but to get rid of airplane trips and walking, I need to filter on the Trip level rather than the Summary level. 

Life got in the way of getting this done in a timely manner but because I looked at this with a fresh set of eyes I realized today that I had a few things mixed up. There were a couple duplicate lines I needed to abstract to a service, and then I realized some things depended on others in a way that wasn't really solid. So I cleaned those up real quick, stopped my kids from fighting, almost finished it up, bedtime, ok now it's ready. The UserDataCollectionService would be a candidate for also splitting up into something that manages data intake and something that retrieves sensible sets of data but I think for the scope of this project that would be going "Overboard" which the problem statement suggested we not do.

Execution Instructions:
Windows: cd into the DrivingData\bin\Release\netcoreapp2.1\win10-x64 directory and do, e.g.
DrivingData.exe ExampleTextFiles/ExampleFromRoot.txt
or you if you prefer Linux, go to DrivingData\bin\Release\netcoreapp2.1\ubuntu.16.10-x64
DrivingData ExampleTextFiles/ExampleFromRoot.txt (haven't tested this one, but the internet says it should work, and everything you read on the internet is true)

I built for two common platforms because Tim suggested file size was a better priority over a self-contained package. The files I am sending depend on having .net but, since it's written in .net core, it should build a self-contained app for about 60MB. 
