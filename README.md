# ServerWatch
ServerWatch was developed as part of my Master’s thesis in Software Engineering at Sapientia Hungarian University of Transylvania.

_Lightweight, extensible server health monitoring and alerting for Windows environments_

## Overview

ServerWatch is a .NET Framework 4.7.2–based application that continuously monitors key aspects of your Windows servers (disk health, database mirroring status, backup save results) and presents them in a real-time WPF dashboard.

## Key Features

- **Health Checks**  
  - Disk health
  - SQL Server database mirroring statuses  
  - Verification of recent backup saves  
- **Dashboard**  
  - WPF-based UI showing live metrics and historical trends  
  - Details per server

## Tech Stack

- **Language & Runtime:** C# on .NET Framework 4.7.2  
- **Database:** Microsoft SQL Server (for storing metrics & history)  
- **Frontend:** WPF (.NET)  

## Prerequisites

- **Operating System:** Windows 10 / Windows Server 2016 or later  
- **.NET Framework:** 4.7.2  
- **Database:** SQL Server 2008 R2 + (Express or full edition)  
- **Permissions:**  
  - Service account with admin access
  - SQL login with permissions to create and write to the ServerWatch database
