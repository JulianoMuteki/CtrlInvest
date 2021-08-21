# CtrlInvest
investment robot

### AdminLTE Template
AdminLTE has been carefully coded with clear comments in all of its JS, SCSS and HTML files. SCSS has been used to increase code customizability.
Download from GitHub releases.
https://github.com/ColorlibHQ/AdminLTE

# Migrations Overview

## Open Package Manager Console > ctrlinvest\src\CtrlInvest.Infra.Context
### Create DB
```Add-Migration InitialCreateDB```

### Update DB
```Update-database```

READ MORE: https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx

## Open PDADMIN

### Create-server in pgADMIN
Host name: ctrlinvest-postgres
Port: 5432
Maintenance database: ctrlinvestDB
username: postgres
password: Look at docker-compose

### Page
http://localhost:16544/