# OTM Client

A C# client API for querying/modifying data on OTM (<https://onlineterritorymanager.com>).

This client API provides programmatic access to a subset of the territory management features of OTM.  This client does not merely act as a wrapper for a public web API, as at present, no such API is available. Instead it does its job by simulating form submissions and parsing HTML content into strongly-typed objects.

Despite what is shown in the examples below, it is highly recommended that OTM Client be implemented as a singleton service.

Although not denoted by naming convention, all OTM Client methods are asynchronous.

## Authentication

## Get A Session Token

```C#
using BWJ.Web.OTM;

//...

// instanciate OTM client
var client = new OtmClient();

// get a session token by providing a valid OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

if(sessionToken is null)
{
    // recommended: handle sign in failure
}
```

> **Note:**
> Always be certain to securely store your OTM credentials.

## Territories

### Get All Territory Information

Get an enumerable collection of all territory information for the organization (language group or congregation) that the owner of the provided session token is associated with.

```C#
using BWJ.Web.OTM;
using BWJ.Web.OTM.Models;
using System.Collections.Generic;

//...

// instanciate OTM client
var client = new OtmClient();

// get a session token by providing a valid OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// get all territory info
IEnumerable<ITerritory> allTerritories =
                await client.Territory.GetTerritoryInfo(sessionToken);
```

> **Note:**
> This API requires a session token belonging to an OTM account admin.

### Get Territory PDF Document

Get data stream containing a printable territory "map card" in PDF format.

```C#
using BWJ.Web.OTM;
using System.IO;

//...

// instanciate OTM client
var client = new OtmClient();

// get a session token by providing a valid OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// This value identifies the record of an assignment of a certain territory to a certain publisher
// It can be found on ITerritory.AssignmentInfo.AssignmentId
// Note that ITerritory.AssignmentInfo is null on territories not currently assigned
int territoryAssignmentId = 1234;

// get the territory map card PDF file stream
using Stream pdfFileStream =
    await client.Territory.GetTerritoryDocument(sessionToken, territoryAssignmentId);

// create a file on disk
using var pdfFile = File.Create(@"C:\file\save\destination\territory.pdf");
// ensure pointer is at the beginning of the stream
pdfFileStream.Seek(0, SeekOrigin.Begin);
// copy stream contents to disk
await pdfFileStream.CopyToAsync(pdfFile);
```

> **Note:**
> To obtain a document for a territory not checked out to the owner of the session token being used, this API requires a session token belonging to an OTM account admin.

### Check In Territory

Return currently assigned territor(y|ies), making such available again for check out or optionally, reassigning to another publisher.

Check in a single territory:

```C#
using BWJ.Web.OTM;

//...

// instanciate OTM client
var client = new OtmClient();

// get a session token by providing a valid OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// This value identifies the record of an assignment of a certain territory to a certain publisher
// It can be found on ITerritory.AssignmentInfo.AssignmentId
// Note that ITerritory.AssignmentInfo is null on territories not currently assigned
int territoryAssignmentId = 1234;

// user account ID for the publisher the checked in territory will be reassigned to
int publisherId = 456123;

// check in and reassign territory -- this argument is optional
await client.Territory.CheckInTerritory(sessionToken, territoryAssignmentId, reassignTo: publisherId);
```

Check in multiple territories:

```C#
using BWJ.Web.OTM;
using System.Collections.Generic;

//...

// instanciate OTM client
var client = new OtmClient();

// get a session token by providing a valid OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// These values identify the records of assignments of territories to publishers
int[] territoryAssignmentIds = { 12300, 45600, 78900 };

// check in territories
// Optionally, a publisher ID argument may be provided
// to reassign these to the given publisher as shown above
await client.Territory.CheckInTerritories(sessionToken, territoryAssignmentIds);
```

> **Note:**
> This API requires a session token belonging to an OTM account admin.

## Get Congregation / Language Group Information

```C#
using BWJ.Web.OTM;
using BWJ.Web.OTM.Models;

//...

// instanciate OTM client
var client = new OtmClient();

// sign in using your OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// get information about the organization (language group or congregation) that the session token owner is associated with
IOrganization myCongregationInfo = await client.Tools.Group.GetSignedInOrganization(sessionToken, bypassCache: false);
```

This API may be called internally by other client API.  When used internally, the returned organization information is cached by session token to minimize network calls. As shown in the example, an argument for the optional `bypassCache` parameter may be provided to force this API use, or not use, cached organization info.  By default `bypassCache` is false, meaning that a cached result will be returned if available.

> **Note:**
> This API requires a session token belonging to an OTM account admin.

## Get Congregation / Language Group User Accounts

```C#
using BWJ.Web.OTM;
using BWJ.Web.OTM.Models;
using System.Collections.Generic;

//...

// instanciate OTM client
var client = new OtmClient();

// sign in using your OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// get a enumerable list of user account information for all users in the
// organization (language group or congregation) that the session token owner is associated with
IEnumerable<IOrganizationUser> congregationUsers =
    await client.Tools.Users.GetOrganizationUsers(sessionToken);
```

> **Note:**
> This API requires a session token belonging to an OTM account admin.

## Verify Account Username Is Unique

```C#
using BWJ.Web.OTM;

//...

// instanciate OTM client
var client = new OtmClient();

// sign in using your OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// returns true if the given user name is unique, otherwise returns false
bool usernameIsUnique = await client.Tools.Users.IsUsernameUnique("username", sessionToken);
```

> **Note:**
> This API requires a session token belonging to an OTM account admin.

## Create A New User Account

```C#
using BWJ.Web.OTM;
using BWJ.Web.OTM.Models;

//...

// instanciate OTM client
var client = new OtmClient();

// sign in using your OTM account user name and password
string sessionToken = await client.Authentication.SignInUser("myUsername", "myPassword");

// returns true if the given user name is unique, otherwise returns false
bool usernameIsUnique = await client.Tools.Users.IsUsernameUnique("username", sessionToken);
```

> **Note:**
> This API requires a session token belonging to an OTM account admin.
>
> Usage of the `IsUsernameUnique` API to ensure the username being submitted in the request is safe to use is highly recommended.
