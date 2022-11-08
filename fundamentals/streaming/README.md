# ASP.NET Core Streaming Example

This repository contains sample code for the article "Streaming Responses in ASP.NET Core" in the ASP.NET Core documentation.

## What

The project contains three implementations of the definition of a basic ASP.NET Core web site that contains endpoints that play a song:

* `AsynchronousWithSystemTextJson`
* `SynchronousWithNewtonsoftJson`
* `DynamicBinaryStream`

These endpoints play the song indefinitely, streaming the song repeatedly until the client disconnects. This acts as a demonstration of how to implement streaming endpoints with ASP.NET Core.

## Compiling

This project can be compiled in Visual Studio, or with the `dotnet` command-line tool:

> `dotnet build`

## Running

The project can be launched with Visual Studio or with the `dotnet` command-line tool:

> `dotnet run`

## Using

There are three examples that show alternative implementations. With an example server running, you can use a browser window or command-line tools to inspect output from the hosted endpoints. Append the desired endpoint onto the base URI displayed in the console where the server is running.

* Browser: Browse to the URL. Some browsers may spool up some of the response before they begin displaying it in realtime.
* PowerShell: _There is no built-in function in PowerShell that can display a web response in realtime._
* Curl: `curl URL --no-buffer`
    * NB: Windows 10 ships with a native port of `curl.exe` -- specify the extension if calling from PowerShell, since PowerShell by default aliases `curl` to `Invoke-WebRequest`.
* Wget: `wget -qO- URL`

### `AsynchronousWithSystemTextJson`

When running, this project presents a web server with a single endpoint.

* Controller endpoint: `/v1/sing`

This project hosts itself on port 5100. A sample URL is:

* [http://localhost:5100/v1/sing](http://localhost:5100/v1/sing)

### `SynchronousWithNewtonsoftJson`

When running, this project presents a web server with two endpoints. These endpoints have identical behavior, but one is implemented using an ASP.NET Core Controller and the other using ASP.NET Core Middleware.

* Controller Endpoint: `/v1/sing`
* Middleware Endpoint: `/middleware/sing`

This project hosts itself on port 5000. Sample URLs are:

* [http://localhost:5000/v1/sing](http://localhost:5000/v1/sing)
* [http://localhost:5000/middleware/sing](http://localhost:5000/middleware/sing)
