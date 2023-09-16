CREATE TABLE Languages (
    Id   INTEGER      PRIMARY KEY AUTOINCREMENT,
    Name VARCHAR (50) 
);

CREATE TABLE Songbooks (
    Id              INTEGER       PRIMARY KEY AUTOINCREMENT,
    LanguageId      INTEGER       REFERENCES Languages (Id),
    Name            VARCHAR (300),
    Description     VARCHAR (50),
    UpdatedLastTime DATETIME
);

CREATE TABLE Songs (
    Id         INTEGER       PRIMARY KEY AUTOINCREMENT,
    SongBookId INTEGER       REFERENCES Songbooks (Id) ON DELETE CASCADE,
    Number     INTEGER,
    Title      VARCHAR (300),
    KeyChord   VARCHAR (15) 
);

CREATE TABLE Verses (
    SongId     INTEGER      REFERENCES Songs (Id) ON DELETE CASCADE,
    VerseType  VARCHAR (50),
    VerseOrder INTEGER,
    Text       TEXT
);


