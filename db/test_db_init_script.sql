DROP TABLE IF EXISTS tbl_currency;
CREATE TABLE tbl_currency(
    curr_iso VARCHAR(5),
    curr_name VARCHAR(65)
);

DROP TABLE IF EXISTS tbl_currency_exchange;
CREATE TABLE tbl_currency_exchange(
    base_cur VARCHAR(5),
    target_cur VARCHAR(5),
    exchange_utc DATE,
    exchange_rate FLOAT
);


DROP TABLE IF EXISTS tbl_income;
CREATE TABLE tbl_income(
    cashflow_id INTEGER PRIMARY KEY,
    happen_utc DATE,
    last_modified_utc DATE, 
    amount FLOAT(2),
    curr_iso VARCHAR(5),
    note VARCHAR(257) DEFAULT "",
    type_id INT DEFAULT 0,
    method_id INT DEFAULT 0
);

DROP TABLE IF EXISTS tbl_expense;
CREATE TABLE tbl_expense(
    cashflow_id INTEGER PRIMARY KEY,
    happen_utc DATE,
    last_modified_utc DATE, 
    amount FLOAT(2),
    curr_iso VARCHAR(5),
    note VARCHAR(257) DEFAULT "",
    type_id INT DEFAULT 0,
    method_id INT DEFAULT 0
);

DROP TABLE IF EXISTS tbl_income_type;
CREATE TABLE tbl_income_type(
    type_id INTEGER PRIMARY KEY,
    type_name VARCHAR(65)
);

DROP TABLE IF EXISTS tbl_expense_type;
CREATE TABLE tbl_expense_type(
    type_id INTEGER PRIMARY KEY,
    type_name VARCHAR(65)
);

DROP TABLE IF EXISTS tbl_pay_method;
CREATE TABLE tbl_pay_method(
    method_id INTEGER PRIMARY KEY,
    method_name VARCHAR(65)
);


INSERT INTO tbl_currency (curr_iso, curr_name)
VALUES 
    ('UNK', 'Unknown'),
    ('USD', 'United States Dollar'),
    ('CNY', 'Renminbi'),
    ('SGD', 'Singapore dollar'),
    ('GBP', 'Sterling'),
    ('EUR', 'Euro'),
    ('JPY', 'Japanese Yen');


INSERT INTO tbl_income_type (type_name, type_id)
VALUES ('Unknown', 0);
INSERT INTO tbl_income_type (type_name)
VALUES 
    ('Salary'),
    ('Credit Card Reward'),
    ('Gift');

INSERT INTO tbl_expense_type (type_name, type_id)
VALUES ('Unknown', 0);
INSERT INTO tbl_expense_type (type_name)
VALUES 
    ('Food'),
    ('Grocery'),
    ('Entertain'),
    ('Traffic'),
    ('Rent');

INSERT INTO tbl_pay_method (method_name, method_id)
VALUES ('Unknown', 0);
INSERT INTO tbl_pay_method (method_name)
VALUES 
    ('Credit Card'),
    ('Debit Card'),
    ('Cash'),
    ('Check');

