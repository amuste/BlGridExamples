# Blgrid demos v.1.6.8
Live demos [here](https://www.blgrid.com/)

Nuget package [here](https://www.nuget.org/packages/BlGrid/)

This repository contains several examples to help you get started developing with BlGrid. The documentation is still in development.

You also will find helper classes that make easy integrate BlGrid server side operations with Entity Framework. 
[QueryHelperClasses for EntityFramework](https://github.com/amuste/BlGridExamples/tree/master/samples/BlGrid.Api/Infrastructure/QueryHelpers)

Demos are provided and tested for the following hosting models
* Blazor WebAssembly
* Blazor WebAssembly Hosted
* Blazor Server

A demo project API is provided for real server side operations demo.
SQL bacpac named `BlGrid_20200815_0004.bacpac` is provided to restore SQL Database.

For BlGrid to work you must add Overlays services and BlGrid services.
use AddDnetOverlay and AddBlGrid for that. example [here](https://github.com/amuste/BlGridExamples/blob/master/src/BlGrid.Shared/Infrastructure/Services/ServiceCollectionExtensions.cs)

A simple example is provided with the minimum possible configuration, you can find it [here](https://github.com/amuste/BlGridExamples/blob/master/src/BlGrid.Shared/Pages/Examples/DummyExample.razor)

### Features
* Client side filtering
* Client side advanced filtering
* Client side sorting
* Client side grouping
* Client side pagination
* Server side filtering
* Server side advanced filtering
* Server side sorting
* Server side pagination
* Cell templates
* Cell redimension
* Row selection
* Multi row selection
* Checkbox column
* Columns definition by code