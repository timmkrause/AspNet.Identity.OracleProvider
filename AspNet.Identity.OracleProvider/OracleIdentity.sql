-- Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

CREATE TABLE <DatabaseSchemaName>.users
(
    id              VARCHAR2 (45 BYTE) NOT NULL,
    username        VARCHAR2 (45 BYTE),
    passwordhash    VARCHAR2 (100 BYTE),
    securitystamp   VARCHAR2 (45 BYTE)
)
/

ALTER TABLE <DatabaseSchemaName>.users
ADD CONSTRAINT pk_users_id PRIMARY KEY (id)
/

CREATE TABLE <DatabaseSchemaName>.roles
(
    id     VARCHAR2 (45 BYTE) NOT NULL,
    name   VARCHAR2 (45 BYTE)
)
/

ALTER TABLE <DatabaseSchemaName>.roles
ADD CONSTRAINT pk_roles_id PRIMARY KEY (id);
/

CREATE TABLE <DatabaseSchemaName>.userclaims
(
    id           NUMBER (*, 0) NOT NULL,
    userid       VARCHAR2 (45 BYTE),
    claimtype    VARCHAR2 (100 BYTE),
    claimvalue   VARCHAR2 (100 BYTE)
)
/

ALTER TABLE <DatabaseSchemaName>.userclaims
ADD CONSTRAINT pk_userclaims_id PRIMARY KEY (id)
/

ALTER TABLE <DatabaseSchemaName>.userclaims
ADD CONSTRAINT fk_userclaims_userid FOREIGN KEY (userid)
REFERENCES <DatabaseSchemaName>.users (id) ON DELETE CASCADE
/

CREATE SEQUENCE <DatabaseSchemaName>.seq_userclaims_id
    INCREMENT BY 1
    START WITH 1
    MINVALUE 1
    MAXVALUE 9999999999
    NOCYCLE
    NOORDER
    CACHE 20
/

CREATE OR REPLACE TRIGGER <DatabaseSchemaName>.tr_userclaims_ins
    BEFORE INSERT
    ON <DatabaseSchemaName>.userclaims
    REFERENCING NEW AS new OLD AS old
    FOR EACH ROW
BEGIN
    IF INSERTING
    THEN
        IF :new.id IS NULL
        THEN
            SELECT seq_userclaims_id.NEXTVAL INTO :new.id FROM DUAL;
        END IF;
    END IF;
END;
/

CREATE TABLE <DatabaseSchemaName>.userlogins
(
    userid          VARCHAR2 (44 BYTE) NOT NULL,
    providerkey     VARCHAR2 (100 BYTE),
    loginprovider   VARCHAR2 (100 BYTE)
)
/

ALTER TABLE <DatabaseSchemaName>.userlogins
ADD CONSTRAINT fk_userlogins_userid FOREIGN KEY (userid)
REFERENCES <DatabaseSchemaName>.users (id) ON DELETE CASCADE
/

CREATE TABLE <DatabaseSchemaName>.userroles
(
    userid   VARCHAR2 (45 BYTE) NOT NULL,
    roleid   VARCHAR2 (45 BYTE) NOT NULL
)
/

ALTER TABLE <DatabaseSchemaName>.userroles
ADD CONSTRAINT pk_userroles_id PRIMARY KEY (userid, roleid)
/

ALTER TABLE <DatabaseSchemaName>.userroles
ADD CONSTRAINT fk_userroles_userid FOREIGN KEY (userid)
REFERENCES <DatabaseSchemaName>.users (id)
/

ALTER TABLE <DatabaseSchemaName>.userroles
ADD CONSTRAINT fk_userroles_roleid FOREIGN KEY (roleid)
REFERENCES <DatabaseSchemaName>.roles (id)
/