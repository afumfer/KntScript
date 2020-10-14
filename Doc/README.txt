# KntScript


KntScript is a minimalist and interpreted programming language with the goal of creating scripts for task-automatization on top of the .NET runtime environment. Applications written in KntScript can use .NET execution environment as well as most of the library classes. 

KntScript is based on the project "Good for Nothing Compiler" presented by Joel Pober in the following blogpost: [Create a Language Compiler for the .NET Framework](https://docs.microsoft.com/en-us/archive/msdn-magazine/2008/february/create-a-language-compiler-for-the-net-framework-using-csharp)

KntScript has been created for educational and learning purposes on how to implement compilers. However,  it could also have practical use as it allows developers to embed the applications within a .NET host application (such as Windows Form or WPF). In this way, developers could provide their applications with an automation mini-language for macro-utilities (in the VBA style). This is possible because KntScript can invoke the .NET framework libraries and/or invoke functions from user's applications.

## Setup

KntScript is adapted to .Net Core 3.1.  This repository provides a set of examples and task-automatization scripts. To run examples, you need  Visual Studio 2019 (16.7.5 or higher). 

However, it is possible to port KntScript to be used with older versions of the .NET Framework, 

To discover the qualities of KntScript, you can see the code of the `DemoForm` form of the `KntScriptAppHost` project (a small Windows Form application). There is a set of examples and different ways to use KntScript.

## Some examples


```
' Simple demo example with:
' --- variables
' --- expressions
' --- for loop / foreach loop
' --- while loop
' --- call function library
' --- if statement

' Some variables
var a = 0;
var b = ((5 + 2) * 3) - 1; 
var c = "Some text";
var d = true;
var e = NewCollectionObjects();
var files = System.IO.Directory.GetFiles(".\");

printline "For loop:";

for a = 1 to 5
    c = "Hi, for loop " + a;    
    e.Add(c);
    printline c;
end for;

printline "";
printline "foreach loop:";

foreach f in files
	printline f;
end foreach;

printline "";
printline "While loop / if statement / call library method:";

a = 0;
' call RandomInt (function library)
b = RandomInt(10,20);

while (a < 50)
    if a > b then
        printline " >> BREAK AT " + a;
        break;
    end if;
    printline "Hi, while loop with break " + a;
    a = a + 1;
end while;
```

```
' --- Send an email using .NET objects
'     (You need to customize your email, pwd, ...).

' Customize your account and credentials here ---------
var user = "fooUserxx";
var pwd = "tutuPwdxx";
var fromEmail = "foo@mycompany.com";
var fromName = "Mr. Foo"
var toEmail = "tutuxx@mycompany.com";
var host = "xxx.myhost.com";
var port = 25;
' -----------------------------------------------------

var msg = new System.Net.Mail.MailMessage(); 
var client = new System.Net.Mail.SmtpClient(); 

printline "Sending message ...  ";

msg.To.Add(toEmail);
msg.From = new System.Net.Mail.MailAddress(fromEmail , fromName); 
msg.Subject = "Subject: test KntScript, sending message"; 
msg.Body = "Body: test KntScript, sending message"; 
msg.IsBodyHtml = false;              

client.Credentials = new System.Net.NetworkCredential(user, pwd);
client.Port = port;
client.Host = host;
client.EnableSsl = true;

client.Send(msg);            

printline "--- email sent ---";

printline  "<< end >>";
```

```
' --- Example obtain data from SQL Server (store procedure)
'     (You need to customize your server, database and store procedure).

var db = GetSQLConnection("Server=.\SQLEXPRESS;Database=MyDataBase;trusted_connection=Yes");
var cmd = new System.Data.SqlClient.SqlCommand("sp_tables", db);
cmd.CommandType = System.Data.CommandType.StoredProcedure;

var param = new System.Data.SqlClient.SqlParameter("@table_owner", System.Data.SqlDbType.VarChar);
param.Value = "dbo";

SetParameter(cmd, param);

db.Open();
var reader = cmd.ExecuteReader();

while reader.Read()                        
	printline reader.GetString(2);
end while;

reader.Close();
db.Close();

printline "";		
printline "<< end >>";
```




### Some references:

- GfnCompiler by Joel Pobar 
https://docs.microsoft.com/en-us/archive/msdn-magazine/2008/february/create-a-language-compiler-for-the-net-framework-using-csharp
https://github.com/johandanforth/good-for-nothing-compiler

- Basic.NET by Alejandro Miralles 
(Missing links  :-( ... )
