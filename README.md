
# Jobbr RavenDB Storage Provider [![Develop build status][ravendb-badge-build-develop]][ravendb-link-build]

This is a storage adapter implementation for the [Jobbr .NET JobServer](http://www.jobbr.io) to store job related information in RavenDB. 
The Jobbr main repository can be found on [JobbrIO/jobbr-server](https://github.com/jobbrIO).

[![Master build status][ravendb-badge-build-master]][ravendb-link-build] 
[![NuGet-Stable][ravendb-badge-nuget]][ravendb-link-nuget]
[![Develop build status][ravendb-badge-build-develop]][ravendb-link-build] 
[![NuGet Pre-Release][ravendb-badge-nuget-pre]][ravendb-link-nuget] 

## Installation
First of all you'll need a working jobserver by using the usual builder as shown in the demos ([jobbrIO/jobbr-demo](https://github.com/jobbrIO/jobbr-demo)). In addition to that you'll need to install the NuGet Package for this extension.

### NuGet

    Install-Package Jobbr.Storage.RavenDB

### Configuration
To configure the Jobbr server to use RavenDB, use the AddRavenDbStorage extension method on JobbrBuilder:

```c#
using Jobbr.Storage.RavenDB;

/* ... */

var builder = new JobbrBuilder();

builder.AddRavenDbStorage(config =>
{
    config.Url = "http://localhost:8080";
    config.Database = "Jobbr";
});

server.Start();
```

# License
This software is licenced under GPLv3. See [LICENSE](LICENSE), and the related licences of 3rd party libraries below.

# Acknowledgements
This extension is built using the following great open source projects

* [RavenDB](https://github.com/ravendb/ravendb) 



# Credits
This application was built by the following awesome developers:
* Michael Schnyder
* Oliver Zürcher

[ravendb-link-build]:            https://ci.appveyor.com/project/Jobbr/jobbr-storage-ravendb         
[ravendb-link-nuget]:            https://www.nuget.org/packages/Jobbr.Storage.RavenDB

[ravendb-badge-build-develop]:   https://img.shields.io/appveyor/ci/Jobbr/jobbr-storage-ravendb/develop.svg?label=develop
[ravendb-badge-build-master]:    https://img.shields.io/appveyor/ci/Jobbr/jobbr-storage-ravendb/master.svg?label=master
[ravendb-badge-nuget]:           https://img.shields.io/nuget/v/Jobbr.Storage.RavenDB.svg?label=NuGet%20stable
[ravendb-badge-nuget-pre]:       https://img.shields.io/nuget/vpre/Jobbr.Storage.RavenDB.svg?label=NuGet%20pre

