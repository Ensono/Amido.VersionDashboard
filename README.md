**Version Dashboard** is a small website that quite simply a collection of boxes that represent the services you have deployed.  These panels describe the type, location and critically the current version of the software as it is deployed.  In and of itself the version dashboard is useless without the capability for each service to be able to expose it's version number to consumers.

## Setup Process

The project is setup in such a way that you can launch the version dashboar without a backing database, simply compile and run the solution in the "Debug" Build Configuration.  This engage the "FileSystem" Database and Network Stack this is the very same system I use for day to day development as it represents all services as JSON files on disk, no actual network connections are made and thus there is no concerns about the security of a database.

It's only once you compile and run the solution in the "Release" Build Configuration that the version Dashboard will attempt to connect to an Azure DocumentDB Database and make actual network calls.  You can of course customize this behaviour by swapping out the mappings in NinjectWebCommon.cs with your own implementations of the Data Store and Request Proxy.

## Todo

The list below is basically a long, rambling list of things that still could be done on the project.  They are in no particular order and may or may not ever be attempted, if you would like to see something else added then please feel free to <a href="mailto:richard.slater@amido.com">drop me an e-mail</a>.

 * Extract validation of calls to proxy out into separate injectable service.
 * Add support for validating the JSON path is allowed into a separate service.
 * Add caching to DocumentDB Store.
 * Create schema for JSON dashboard documents to ease the creation.
 * Add ability to edit existing Dashboards.
 * Add support for creating new dashboards.
 * Add support for caching of requests made by the Request Proxy, currently we have a scalability problem.
 * Add Unit Tests for all of the services created so far.
 * Add support for an Azure WebJob to poll the version endpoints across all dashboards on a regular cadence and store the results of these requests in a data store.
 * Add support for rendering the version history of any given version endpoint to map the progression of versions through a group of environments.
 * Add "Deploy to Azure" button that deploys the solution in "FileSystem Mode".
 * Add "Deploy to Azure" button that deploys the solution in "Production Mode".
 * Create new DataStores for other Services:
  * Blob Storage
  * MongoDB
 * Add support for other Request Proxies
  * XML/XPath
  * Form Encoded/Key Value Pair
  * YAML