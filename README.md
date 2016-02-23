# TInjector
A simple .NET IoC container with only the features needed for correctly-designed applications.

## Design Decisions
* Only support features necessary for properly designed apps 
	* No extensible scopes (prevent messy scope-dependant code)
	* Once a root (container/kernel/etc.) is created lock it against further modification
	* Do not support automatic registration of concrete classes (you should always wrap it in an interface so you can test things that use it)
* High concurrency (lock as little as possible)
* Support heavy automation (automatic registration wherever reasonable)
* Testability (unit test all the code, create an api that allows extensive unit testing of anything using it)
* Simplicity (of the code, of the api, of the documentation, everywhere)
* Documented code (XML docs on all protected or public code, notes inside code on pretty much every line)

## TODO:
* Support constants
* Support value types
* Support getting lists of implementations
* Support activation callbacks
* Support decorators (implements the same interface and takes the base implementation as a constructor param)
