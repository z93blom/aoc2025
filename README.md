
# Advent of Code (2025)
My C# solutions to the advent of code problems.
Check out http://adventofcode.com/2025.

# Credit
The source template of this repository was (probably - memory is a bit hazy) taken from 
https://github.com/encse/adventofcode. It has been modified over the years.

# Automation
This repo does follow the automation guidelines on the /r/adventofcode community wiki 
[https://www.reddit.com/r/adventofcode/wiki/faqs/automation] 

Specifically:

Outbound calls are throttled (Not considered relevant, since the scripts are only run manually.)

Once inputs are downloaded, they are cached locally. See [Framework/Updater.cs - UpdateInput method](Framework/Updater.cs).

The User-Agent header in userAgentHeaderFunction() is required to be set in order to fetch
data. See [Framework/Updater.cs - usage of UserAgentEnvironmentName](Framework/Updater.cs).
It is currently set to point back to this repo and my email address.

# Copyrights
The source template by https://github.com/encse/adventofcode was MIT at the time of writing/copying.
The https://adventofcode.com/2025/about about page states that the inputs and the puzzle are 
not to be added to the repository. A quick summary of the puzzle is created, and the input files
are not tracked in the repository.

# Setting the session variables
The application depends on two environment variables. 

To get your session identifier, you need to
look at the request headers for your daily data, and take the session identifier from
the cookie in the Request Headers (for example https://adventofcode.com/2025/day/1/input)

Then you can set it:

```
Powershell: $Env:AOCSESSION = "..."

Command Prompt: set AOCSESSION = ...
```

The second environment variable is used to set the UserAgent header for the requests
I've used the rquested format: `github.com/z93blom/aoc2025 by <my mail address>`

```
Powershell: $Env:AOCUSERAGENT = "..."

Command Prompt: set AOCUSERAGENT = ...
```


## Running

To run the project:

1. Install .NET
2. Clone the repo
3. Get help with `dotnet run`
```
Usage: dotnet run [arguments]
Supported arguments:

solve [year]-[day]    Solve the specified problems
solve [year]          Solve the whole year
solve today           Solve today's problem (only available during event).

To start working on new problems:

update [year]-[day]   Prepares a folder for the given day, updates the input, 
					  the readme and creates a solution template.
update today          Same as above, but for the current day. (only available during event). 

Useful commands during december:
dotnet run update today
dotnet run solve today 

dotnet run update calendar
dotnet run calendar

```
