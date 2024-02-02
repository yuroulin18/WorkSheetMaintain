create database IsSQL

use IsSQL

create table employee(
	id int,
	account varchar(50),
	password varchar(256),
	pass varchar(50)
);

insert into employee
VALUES(1,'stanley',substring(sys.fn_sqlvarbasetostr(HashBytes('MD5','JoXAr3')),3,32),'JoXAr3');

insert into employee
VALUES(2,'newaurora',substring(sys.fn_sqlvarbasetostr(HashBytes('MD5','NGL1ZSkd2O0&nKHQF!x1')),3,32),'NGL1ZSkd2O0&nKHQF!x1');

insert into employee
VALUES(3,'stanley543',substring(sys.fn_sqlvarbasetostr(HashBytes('MD5','O0&eCX')),3,32),'O0&eCX');

insert into employee
VALUES(4,'newaurora543',substring(sys.fn_sqlvarbasetostr(HashBytes('MD5','g&JSc#')),3,32),'g&JSc#');

select *
from employee

--' OR ''=''-- 