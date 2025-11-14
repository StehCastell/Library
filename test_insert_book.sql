-- Teste: Inserir um livro de exemplo no banco de dados
-- Primeiro, verifique se há usuários cadastrados:
SELECT * FROM users;

-- Se houver um usuário com ID 1, insira um livro de teste:
INSERT INTO books (title, author, genre, pages, type, status, user_id, created_at)
VALUES ('Test Book', 'Test Author', 'Fiction', 300, 'Physical', 'not-read', 1, NOW());

-- Verifique se o livro foi inserido:
SELECT * FROM books;
