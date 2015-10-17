# TInjector
A simple .NET IoC container with only the features needed for correctly-designed applications.

## Design Decisions

* Only support features necessary for properly designed apps (prevent messy scope-dependant code)
* No extensible scopes! (See above)
* Once a root (container/kernel/etc.) is created lock it against further modification
* High concurrency (lock as little as possible)
* Testability (unit test all the code, create an api that allows extensive unit testing of anything using it)
* Simplicity (of the code, of the api, of the documentation, everywhere)
* Documented code (XML docs on all public code, notes inside code on pretty much every line)
* Localizable code (no embedded strings!)
