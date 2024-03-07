# Risk based programming

Examples for the Risk Based Programming

Potential outcomes of risk areas:
- Unexpected behavior - something that engineering hasn't considered (exception is thrown when not expected)
- Undefined behavior - a behavior that wasn't defined by the engineer (say if statement is true, but else is not handled)
- Inconsistent behavior - a behavior that differs according to other factors (time, threads, network, etc)

Solution to have:
- Clearly defined code that exhibits consistent behavior with every execution.
- This has be established, during:
    - Designing
    - Implementation
    - Testing
    - Validation

## Code

What codes are typically high-risk:
- Anything custom written, generally has a lot of bugs if not tested properly
- Web APIs - so many things go wrong (timeouts, throttling, delays, unexpected changes, HTML vs JSON response, corruption, auth failures, etc)
- Multithreading code - very difficult to get it right and test
- IO operations: write/append/delete - high risk, something can go wrong (and many times it did!)
- Security: authentication/authorization/etc - Very hard to get right, and with a significant risk
- Combination of those is a bliss point

## Risk classification

### Risk impact

- Total cost - the damage is considered to be in money. This means a total cost:
- Time to mitigate = (effort * time)
- Additional expenses + resources
- Lost revenue - considering the application is not running and company doesn't make any profits
- Compensations - if damage was made to customers/partners
- Loss of reputation - potential impact in the future

### Risk type

- Data corruption - While application is working, the result is corruption of data, this is difficult to indentify and mitigate
- Security - Potential security impact: data breach, ransomware attack, extortion, takeover control
- App crash - Application stops working completely or becomes unresponsive
- Transient - Happens inconsistently at certain times
- Unexpected - Application executes the code but the results are unexpected
- Visual - Doesn't prevent application from working, displays incorrect or invalid values
- Undetected - Issues that might happen but are not currently identified. It's assumed there are always some defects always present.


## App example

- Demonstrates different situations by injecting failures and showing the impact it can have
- leap bug
- data corruption
- thread failures
- unexpected exceptions, etc

- downloading a file
- storing the file

## Leap year bug:

https://techcommunity.microsoft.com/t5/azure-developer-community-blog/it-s-2020-is-your-code-ready-for-leap-day/ba-p/1157279

## Plugins

- For Visual Studio - Fine Code Coverage
