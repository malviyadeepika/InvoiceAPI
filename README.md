# 📄 Invoice API

## Overview
The **Invoice API** is a RESTful web service for managing invoices. It allows users to:
- Create new invoices 
- Retrieve all invoices 
- Pay invoices 
- Process overdue invoices 

This project is built using **ASP.NET Core Web API**.

---

## Features
 **Create Invoice**: Add a new invoice with an amount and due date.  
 **Get Invoices**: Retrieve all invoices with their details.  
 **Pay Invoice**: Make a payment towards an invoice.  
 **Process Overdue Invoices**: Marks overdue invoices as "paid" or "void" and generates new ones with late fees.  
 **Docker Support**: A `docker-compose.yml` file is included to run the API in a container.  

---

## Tech Stack
- **Backend**: ASP.NET Core Web API  
- **Tools**: Swagger, Docker, xUnit (for testing)  
- **Version Control**: Git & GitHub  
