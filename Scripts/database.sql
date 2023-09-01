create database blogEngine; 

create table roles (
    id int IDENTITY(1,1) PRIMARY KEY,
    name varchar(100) not null,
    active BINARY not null,
);

create table users (
    id int IDENTITY(1,1) PRIMARY KEY,
    name varchar(100) not null,
    lastname varchar(100) not null,
    email varchar(100) not null,
    role int not null, 
    password varchar(100) not null, 
    foreign key (role) references roles(id)
);

create table posts (
    id int IDENTITY(1,1) PRIMARY KEY,
    title varchar(100) not null,
    content text not null,
    author int not null,
    created_at datetime not null,
    updated_at datetime not null,
    foreign key (author) references users(id)
);

create table comments (
    id int IDENTITY(1,1) PRIMARY KEY,
    content text not null,
    author int not null,
    post int not null,
    foreign key (author) references users(id),
    foreign key (post) references posts(id)
);

create table status (
    id int IDENTITY(1,1) PRIMARY KEY,
    name varchar(100) not null
);

create table post_status (
     id int IDENTITY(1,1) PRIMARY KEY,
    post int not null,
    comment_editor varchar(100) not null,
    created_at datetime not null,
    status int not null,
    foreign key (post) references posts(id),
    foreign key (status) references status(id)
); 