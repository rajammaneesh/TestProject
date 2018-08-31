INSERT INTO `dcode`.`task_type`
(`ID`,
`DESCRIPTION`,
`CODE`)
VALUES
(3,
'Industry Initiative',
'II');

CREATE TABLE `dcode`.`portfolio_tasktype` (
  `portfolioId` INT NOT NULL,
  `tasktypeId` INT NOT NULL,
  PRIMARY KEY (`portfolioId`, `tasktypeId`),
  INDEX `FK_TaskType_idx` (`tasktypeId` ASC),
  CONSTRAINT `FK_Portfolio`
    FOREIGN KEY (`portfolioId`)
    REFERENCES `dcode`.`portfolios` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_TaskType`
    FOREIGN KEY (`tasktypeId`)
    REFERENCES `dcode`.`task_type` (`ID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('1', '1');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('2', '1');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('3', '1');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('4', '1');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('5', '1');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('6', '1');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('1', '2');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('2', '2');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('3', '2');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('4', '2');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('5', '2');
INSERT INTO `dcode`.`portfolio_tasktype` (`portfolioId`, `tasktypeId`) VALUES ('6', '2');

  `Id` INT NOT NULL AUTO_INCREMENT,
  `PortfolioId` INT NOT NULL,
  `TaskTypeId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  INDEX `FK_Portfolio_idx` (`PortfolioId` ASC),
  INDEX `FK_TaskType_idx` (`TaskTypeId` ASC),
  CONSTRAINT `FK_Portfolio`
    FOREIGN KEY (`PortfolioId`)
    REFERENCES `dcode`.`portfolios` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_TaskType`
    FOREIGN KEY (`TaskTypeId`)
    REFERENCES `dcode`.`task_type` (`ID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
