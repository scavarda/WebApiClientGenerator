# Scavarda.WebApiClientGenerator

CLI application for automatic generation of WebApi Client from a swagger.json file.

This CLI wraps [swagger-codegen CLI](https://github.com/swagger-api/swagger-codegen) and [dotnet CLI](https://docs.microsoft.com/it-it/dotnet/core/tools/?tabs=netcore2x) 

## Requirements
NetCore 2.1

## Supported client languages
- .NET 4.0 
- .NET 4.5
- Java
- NetStandard 2.0
- PHP
- Typescript-Angular

## Documentation for CLI Commands

Main
```
Generate WebApi Client from swagger.json file

Usage: Scavarda.WebApiClientGenerator [options] [command]

Options:
  --version        Show version information
  -h|--help        Show help information

Commands:
  build-package    Build package for WebApi Client from swagger.json file
  generate-client  Generate WebApi Client from swagger.json file

Run 'Scavarda.WebApiClientGenerator [command] --help' for more information about a command.
```

build-package 
```
Build package for WebApi Client from swagger.json file

Usage: Scavarda.WebApiClientGenerator build-package [options]

Options:
  -h|--help                               Show help information
  -i|--input <path>                       Full path of swagger.json file
  -l|--language <language>                WebApi client target language (One of: all, dotnet4, dotnet45, java, netcore2, php, typescript-angular)
  -o|--output <output_folder>             WebApi client package output folder
  -n|--package-name <package_name>        WebApi client package name
  -v|--package-version <package_version>  WebApi client package version
```

generate-client
```
Generate WebApi Client from swagger.json file

Usage: Scavarda.WebApiClientGenerator generate-client [options]

Options:
  -h|--help                               Show help information
  -i|--input <path>                       Full path of swagger.json file
  -l|--language <language>                WebApi client target language (One of: all, dotnet4, dotnet45, java, netcore2, php, typescript-angular)
  -o|--output <output_folder>             WebApi client source code output folder
  -n|--package-name <package_name>        WebApi client package name
  -v|--package-version <package_version>  WebApi client package version
```