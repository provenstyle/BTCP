# BibleTraining
Database for BibleTraining students and classes

### Set up your environment

#### Node
Node is used during development to build and serve the css, html, and javascript

Download and install [node](https://nodejs.org) from their website.

Install the [gulp-cli](http://gulpjs.com) globally.
```
$ npm install -g gulp-cli
```

Install [bower](http://bower.io) globally.
```
$ npm install -g bower
```

Install [karma-cli](https://www.npmjs.com/package/karma-cli) globally.
```
$ npm install -g karma-cli
```

#### IIS
IIS needs to be enabled on your machine

#### VisualStudio
This application was written using Visual Studio 2015 Service Pack 3

Install [VisualStudio](https://www.visualstudio.com/vs/)

#### Sql Server
Install [SqlServer](https://www.microsoft.com/en-us/sql-server/sql-server-2016)

I started the application with sql server 2014 but and now using 2016

### Install application dependencies
Install npm dependencies
```
$ npm install
```

Install bower dependencies
```
$ bower install
```

### Setup IIS Virtual Directory

1. Open BibleTraining.sln
2. Go to BibleTraining.Web.UI properties > Web
3. Select Local IIS
4. Set the project url to http://localhost/BibleTraining
5. Click Create Virtual Directory

### Build solution in Visual Studio 
Build > Build Solution

### Install the local database

1. Set BibleTraining.Migrations as startup project
2. Run project to install/update database
3. If errors occur set a breakpoint in the program.cs to see the console output

### Run Application
```
gulp serve
```

### Watch code and run build pipeline
```
gulp watch
```

### Execute Tests
```
karma start
```
