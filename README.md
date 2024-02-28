# Risk based programming

Examples for the Risk Based Programming

Potential outcomes of risk areas:
- Unexpected behavior - something that engineering hasn't considered
- Undefined behavior - a behavior that wasn't defined by the engineer
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
- Multithreading code - very difficult to get it right


## Leap year bug:

https://techcommunity.microsoft.com/t5/azure-developer-community-blog/it-s-2020-is-your-code-ready-for-leap-day/ba-p/1157279


