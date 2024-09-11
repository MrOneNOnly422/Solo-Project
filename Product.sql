﻿CREATE TABLE product (
    ProductID INT PRIMARY KEY,
    BookName VARCHAR(100) NOT NULL,
    Author VARCHAR(100) NOT NULL,
    Stock INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
