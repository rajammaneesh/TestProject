CREATE DATABASE  IF NOT EXISTS `dcode`;
drop database dcode;
create database dcode;
use dcode;

Create table log(
Id int NOT NULL auto_increment,
description varchar(255),
details varchar(255),
user varchar(50),
primary key (ID)
) auto_increment=1;


Create TABLE USERS(
	ID INT NOT NULL auto_increment,
	FIRST_NAME varchar(50) NULL,
	LAST_NAME varchar(50) NULL,
	DESIGNATION varchar(50) NULL,
	EMAIL_ID varchar(255) NOT NULL,
	MANAGER_EMAIL_ID varchar(255) NULL,
	STATUS varchar(20) NULL,
	STATUS_DATE datetime NULL,
	CREATED_BY varchar(100) NULL,
	CREATED_ON datetime NULL,
	UPDATED_BY varchar(100) NULL,
	UPDATED_ON datetime NULL,
PRIMARY KEY(ID)
) auto_increment=1;

CREATE TABLE TASKS(
	ID int NOT NULL auto_increment,
	USER_ID INT NOT NULL,
	PROJECT_NAME varchar(50) NOT NULL,
	TASK_NAME varchar(50) NULL,
	PROJECT_WBS_Code varchar(50) NOT NULL,
	TYPE varchar(100) NULL,
	DETAILS varchar(255) NULL,
	HOURS int NULL,
	COMMENTS varchar(255) NULL,
	GIFTS bit NULL,
	ONBOARDING_DATE datetime NULL,
	DUE_DATE datetime NULL,
	STATUS varchar(20) NULL,
	STATUS_DATE datetime NULL,
	CREATED_BY varchar(100) NULL,
	CREATED_ON datetime NULL,
	UPDATED_BY varchar(100) NULL,
	UPDATED_ON datetime NULL,
primary key (ID),
foreign key (USER_ID) references users(ID)
) auto_increment=1;


CREATE TABLE TASKAPPLICANTS(
	ID INT NOT NULL auto_increment,
	TASK_ID INT NOT NULL,
	APPLICANT_ID INT NOT NULL,
	STATUS varchar(20) NULL,
	STATUS_DATE datetime NULL,
	CREATED_BY varchar(100) NULL,
	CREATED_ON datetime NULL,
	UPDATED_BY varchar(100) NULL,
	UPDATED_ON datetime NULL,
PRIMARY KEY(ID),
FOREIGN KEY(APPLICANT_ID) references USERS(ID),
FOREIGN KEY(TASK_ID) references TASKS(ID)
)auto_increment=1;

CREATE TABLE APPROVEDAPPLICANT(
	ID INT NOT NULL auto_increment,
	APPLICANT_ID INT NOT NULL,
	TASK_ID	int NOT NULL,
	HOURS_WORKED decimal(5, 1) NULL,
	RATING varchar(50) NULL,
	WORK_AGAIN bit NULL,
	POINTS int NULL,
	COMMENTS varchar(255) NULL,
	STATUS varchar(20) NULL,
	STATUS_DATE datetime NULL,
	CREATED_BY varchar(100) NULL,
	CREATED_ON datetime NULL,
	UPDATED_BY varchar(100) NULL,
	UPDATED_ON datetime NULL,
PRIMARY KEY(ID),
FOREIGN KEY(APPLICANT_ID) references USERS(ID),
FOREIGN KEY(TASK_ID) references TASKS(ID)
)auto_increment=1;


CREATE TABLE SKILLS(
	ID INT NOT NULL auto_increment,
	VALUE varchar(50) NOT NULL,
	STATUS varchar(20) NULL,
	STATUS_DATE datetime NULL,
	CREATED_BY varchar(100) NULL,
	CREATED_ON datetime NULL,
	UPDATED_BY varchar(100) NULL,
	UPDATED_ON datetime NULL,
PRIMARY KEY(ID)
)auto_increment=1;

CREATE TABLE APPLICANTSKILLS(
	ID INT NOT NULL auto_increment,
	APPLICANT_ID INT NOT NULL,
	SKILL_ID INT NOT NULL,
	STATUS varchar(20) NULL,
	STATUS_DATE datetime NULL,
	CREATED_BY varchar(100) NULL,
	CREATED_ON datetime NULL,
	UPDATED_BY varchar(100) NULL,
	UPDATED_ON datetime NULL,
PRIMARY KEY(ID),
FOREIGN KEY(APPLICANT_ID) references USERS(ID),
FOREIGN KEY(SKILL_ID) references SKILLS(ID)
)auto_increment=1;


CREATE TABLE TASKSKILLS(
	ID INT NOT NULL auto_increment,
	TASK_ID INT NOT NULL,
	SKILL_ID INT NOT NULL,
	STATUS varchar(20) NULL,
	STATUS_DATE datetime NULL,
	CREATED_BY varchar(100) NULL,
	CREATED_ON datetime NULL,
	UPDATED_BY varchar(100) NULL,
	UPDATED_ON datetime NULL,
PRIMARY KEY(ID),
FOREIGN KEY(TASK_ID) REFERENCES TASKS(ID),
FOREIGN KEY(SKILL_ID) references SKILLS(ID)
)auto_increment = 1;

INSERT INTO USERS(ID,FIRST_NAME,LAST_NAME,DESIGNATION,EMAIL_ID,MANAGER_EMAIL_ID,STATUS,STATUS_DATE,CREATED_BY,CREATED_ON,UPDATED_BY,UPDATED_ON)VALUES
(1,'Ravi','Kumar','Consultant','rkumar@deloitte.com','vthakur@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(2,'Rahul','Goel','Consultant','rgoel@deloitte.com','vthakur@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(3,'Prakash','Singh','Consultant','psingh@deloitte.com','vthakur@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(4,'Likhita','Agarwal','Consultant','lagarwal@deloitte.com','vthakur@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(5,'Kunal','Singh','Consultant','ksingh@deloitte.com','nrathod@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(6,'Kapil','Shah','Consultant','kashah@deloitte.com','nrathod@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(7,'Preeti','Biswas','Consultant','pbiswas@deloitte.com','nrathod@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(8,'Swapnil','Das','Consultant','sdas@deloitte.com','sksoma@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(9,'Sruthi','Shah','Consultant','sshah@deloitte.com','sksoma@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(10,'Kabir','Pal','Consultant','kpal@deloitte.com','sksoma@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(11,'Nayan','Rathod','Manager','nrathod@deloitte.com','vranmu@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(12,'Vishal','Thakur','Manager','vthakur@deloitte.com','vranmu@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(13,'Sai Krishna','Soma','Manager','sksoma@deloitte.com','vranmu@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(14,'Venkat','Ranmu','Senior Manager','vranmu@deloitte.com','svidya@deloitte.com','Active',NOW(),'dcodedev',NOW(),NULL,NULL);

INSERT INTO TASKS(ID,USER_ID,PROJECT_NAME,TASK_NAME,PROJECT_WBS_CODE,TYPE,DETAILS,HOURS,COMMENTS,GIFTS,ONBOARDING_DATE,DUE_DATE,STATUS,STATUS_DATE,CREATED_BY,CREATED_ON,UPDATED_BY,UPDATED_ON) values
(1,11,'Telstra Super','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Assigned',NOW(),'dcodedev',NOW(),NULL,NULL),
(2,11,'VicRoads','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(3,11,'Novonordisk','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(4,12,'Exxon Mobil','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(5,12,'VM Ware','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(6,12,'Amex Solution','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(7,12,'Nu Skin','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(8,11,'David Yurman','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(9,13,'Novartis','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(10,13,'USTA','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(11,13,'Rent a Car','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(12,13,'Stanley','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(13,13,'Maersk','Build CMS solution','WBS_TEL','','Need to build server side code to enable CMS',20,'Need to build server side code to enable CMS',1,CAST(N'2016-07-26 11:24:52.550' AS DateTime),CAST(N'2016-07-26 11:24:52.550' AS DateTime),'Active',NOW(),'dcodedev',NOW(),NULL,NULL);


INSERT INTO SKILLS(ID,VALUE,STATUS,STATUS_DATE,CREATED_BY,CREATED_ON,UPDATED_BY,UPDATED_ON) values
(1,'Dotnet','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(2,'Java','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(3,'AEM','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(4,'Hybris','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(5,'Sharepoint','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(6,'Sitecore','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(7,'Hana','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(8,'Cloudcraze','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(9,'Salesforce','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(10,'UX','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(11,'UI','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(12,'Angularjs','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(13,'Nodejs','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(14,'Polymerjs','Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(15,'Azure','Active',NOW(),'dcodedev',NOW(),NULL,NULL);

insert into taskskills(ID,TASK_ID,SKILL_ID,STATUS,STATUS_DATE,CREATED_BY,CREATED_ON,UPDATED_BY,UPDATED_ON) VALUES
(1,1,1,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(2,2,2,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(3,3,3,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(4,4,4,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(5,5,5,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(6,6,6,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(7,7,7,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(8,8,8,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(9,9,9,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(10,10,10,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(11,11,11,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(12,12,12,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(13,13,13,'Active',NOW(),'dcodedev',NOW(),NULL,NULL);


INSERT INTO taskapplicants(ID,TASK_ID,APPLICANT_ID,STATUS,STATUS_DATE,CREATED_BY,CREATED_ON,UPDATED_BY,UPDATED_ON) values
(1,1,1,'ManagerApproved',NOW(),'dcodedev',NOW(),NULL,NULL),
(2,1,2,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(3,2,3,'ManagerApproved',NOW(),'dcodedev',NOW(),NULL,NULL),
(4,2,4,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(5,3,5,'ManagerApproved',NOW(),'dcodedev',NOW(),NULL,NULL),
(6,3,6,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(7,4,7,'ManagerApproved',NOW(),'dcodedev',NOW(),NULL,NULL),
(8,4,8,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(9,5,9,'ManagerApproved',NOW(),'dcodedev',NOW(),NULL,NULL),
(10,5,10,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(11,6,11,'ManagerApproved',NOW(),'dcodedev',NOW(),NULL,NULL),
(12,6,12,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(13,7,13,'ManagerApproved',NOW(),'dcodedev',NOW(),NULL,NULL),
(14,7,14,'Active',NOW(),'dcodedev',NOW(),NULL,NULL);

INSERT INTO applicantskills(ID,APPLICANT_ID,SKILL_ID,STATUS,STATUS_DATE,CREATED_BY,CREATED_ON,UPDATED_BY,UPDATED_ON) VALUES
(1,1,1,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(2,1,2,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(3,2,1,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(4,2,3,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(5,3,2,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(6,3,4,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(7,4,2,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(8,4,5,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(9,5,3,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(10,5,6,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(11,6,3,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(12,6,7,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(13,7,4,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(14,7,8,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(15,8,4,'Active',NOW(),'dcodedev',NOW(),NULL,NULL),
(16,8,9,'Active',NOW(),'dcodedev',NOW(),NULL,NULL);


SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

CREATE SCHEMA IF NOT EXISTS `dcode` ;
USE `dcode`;

-- -----------------------------------------------------
-- Table `elmah`.`elmah_error`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dcode`.`elmah_error` (
  `ErrorId` CHAR(36) NOT NULL ,
  `Application` VARCHAR(60) NOT NULL ,
  `Host` VARCHAR(50) NOT NULL ,
  `Type` VARCHAR(100) NOT NULL ,
  `Source` VARCHAR(60) NOT NULL ,
  `Message` VARCHAR(500) NOT NULL ,
  `User` VARCHAR(50) NOT NULL ,
  `StatusCode` INT(10) NOT NULL ,
  `TimeUtc` DATETIME NOT NULL ,
  `Sequence` INT(10) NOT NULL AUTO_INCREMENT ,
  `AllXml` TEXT NOT NULL ,
  PRIMARY KEY (`Sequence`) ,
  UNIQUE INDEX `IX_ErrorId` (`ErrorId`(8) ASC) ,
  INDEX `IX_ELMAH_Error_App_Time_Seql` (`Application`(10) ASC, `TimeUtc` DESC, `Sequence` DESC) ,
  INDEX `IX_ErrorId_App` (`ErrorId`(8) ASC, `Application`(10) ASC) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8
CHECKSUM = 1
DELAY_KEY_WRITE = 1
ROW_FORMAT = DYNAMIC;


DELIMITER //

USE dcode//

CREATE PROCEDURE `dcode`.`elmah_GetErrorXml` (
  IN Id CHAR(36),
  IN App VARCHAR(60)
)
NOT DETERMINISTIC
READS SQL DATA
BEGIN
    SELECT  `AllXml`
    FROM    `elmah_error`
    WHERE   `ErrorId` = Id AND `Application` = App;
END//

USE dcode//

CREATE PROCEDURE `dcode`.`elmah_GetErrorsXml` (
  IN App VARCHAR(60),
  IN PageIndex INT(10),
  IN PageSize INT(10),
  OUT TotalCount INT(10)
)
NOT DETERMINISTIC
READS SQL DATA
BEGIN
    
    SELECT  count(*) INTO TotalCount
    FROM    `elmah_error`
    WHERE   `Application` = App;

    SET @index = PageIndex * PageSize;
    SET @count = PageSize;
    SET @app = App;
    PREPARE STMT FROM '
    SELECT
        `ErrorId`,
        `Application`,
        `Host`,
        `Type`,
        `Source`,
        `Message`,
        `User`,
        `StatusCode`,
        CONCAT(`TimeUtc`, '' Z'') AS `TimeUtc`
    FROM
        `elmah_error` error
    WHERE
        `Application` = ?
    ORDER BY
        `TimeUtc` DESC,
        `Sequence` DESC
    LIMIT
        ?, ?';
    EXECUTE STMT USING @app, @index, @count;

END//

USE dcode//

CREATE PROCEDURE `dcode`.`elmah_LogError` (
    IN ErrorId CHAR(36), 
    IN Application varchar(60), 
    IN Host VARCHAR(30), 
    IN Type VARCHAR(100), 
    IN Source VARCHAR(60), 
    IN Message VARCHAR(500), 
    IN User VARCHAR(50), 
    IN AllXml TEXT, 
    IN StatusCode INT(10), 
    IN TimeUtc DATETIME
)
NOT DETERMINISTIC
MODIFIES SQL DATA
BEGIN
    INSERT INTO `elmah_error` (
        `ErrorId`, 
        `Application`, 
        `Host`, 
        `Type`, 
        `Source`, 
        `Message`, 
        `User`, 
        `AllXml`, 
        `StatusCode`, 
        `TimeUtc`
    ) VALUES (
        ErrorId, 
        Application, 
        Host, 
        Type, 
        Source, 
        Message, 
        User, 
        AllXml, 
        StatusCode, 
        TimeUtc
    );
END//

DELIMITER ;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;


