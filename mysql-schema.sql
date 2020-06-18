CREATE TABLE user (firstname VARCHAR(50), lastname VARCHAR(50),
       username VARCHAR(50), status BOOL, email VARCHAR(100), passwordHash VARCHAR(255));

CREATE INDEX ix_username ON user(username);