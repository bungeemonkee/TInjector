# TInjector

A simple .NET IoC container with only the features needed for correctly-designed applications.

__NOTE:__ This is very much still under development. I have already re-written the whole thing several times. I'm not certain I'll ever be happy with its performance or design. It is largely just a platform for me to experiment with and basically deep dive into DI and IOC.

[![Build Status](https://ci.appveyor.com/api/projects/status/jurd99rundkxp8di?svg=true)](https://ci.appveyor.com/project/bungeemonkee/tinjector) [![Coverage Status](https://coveralls.io/repos/github/bungeemonkee/TInjector/badge.svg?branch=master)](https://coveralls.io/github/bungeemonkee/TInjector?branch=master)

## Design Decisions
* Only support features necessary for properly designed apps 
	* No extensible scopes (prevent messy scope-dependant code)
	* Once a root (container/kernel/etc.) is created lock it against further modification
	* Do not support automatic registration of concrete classes (you should always wrap it in an interface so you can test things that use it)
* Fully strongly typed fluent configuration interface
* No reflection in the core lib
    * `typeof(T)` is allowed to get Type instances to be used as dictionary keys, but no properties or methods on Type instances may be called.
* High concurrency (lock as little as possible)
* Support heavy automation (automatic registration wherever reasonable)
    * In some cases this might conflict with not using reflection in the core lib.
      In such cases the reflection is to be moved to a companion library or the feature that needs it is to be removed all together.
* Testability (unit test all the code, create an api that allows extensive unit testing of anything using it)
* Simplicity (of the code, of the api, of the documentation, everywhere)
* Documented code (XML docs on all protected or public code, notes inside code on pretty much every line)
* Portability

## TODO:
* Support decorators? (implements the same interface and takes the base implementation as a constructor param)
* Better method generation in TInjector.Reflection using generated IL (System.Reflection.Emit) or expression trees (System.Linq.Expressions)
