// Load saved theme on start
document.addEventListener('DOMContentLoaded', function () {
    const savedTheme = localStorage.getItem('theme') || 'light';
    document.getElementById('themeSelector').value = savedTheme;
    applyTheme(savedTheme);
});

// Function to change theme
function changeTheme() {
    const theme = document.getElementById('themeSelector').value;
    applyTheme(theme);
    localStorage.setItem('theme', theme);
}

function applyTheme(theme) {
    if (theme === 'dark') {
        document.body.classList.add('dark-theme');
    } else {
        document.body.classList.remove('dark-theme');
    }
}

// Book form submission
document.getElementById('formBook').addEventListener('submit', async function (e) {
    e.preventDefault();

    const editId = document.getElementById('editBookId').value;

    // Get form values
    const bookData = getFormData();

    // Validate before sending
    if (!validateBookData(bookData)) {
        showMessage('bookMessage', 'Please fill in all fields', 'error');
        return;
    }

    // Call appropriate function
    if (editId) {
        await updateBook(editId, bookData);
    } else {
        await addBook(bookData);
    }
});

// Function to get form data
function getFormData() {
    return {
        title: document.getElementById('bookTitle').value.trim(),
        author: document.getElementById('bookAuthor').value.trim(),
        genre: document.getElementById('bookGenre').value.trim(),
        pages: parseInt(document.getElementById('bookPages').value),
        type: document.getElementById('bookType').value,
        status: document.getElementById('bookStatus').value
    };
}

// Function to validate book data
function validateBookData(book) {
    return book.title &&
        book.author &&
        book.genre &&
        book.pages > 0 &&
        book.type &&
        book.status;
}

// Function to add a new book
async function addBook(bookData) {
    console.log('Adding book:', bookData);

    try {
        const response = await fetch('/Books/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(bookData)
        });

        console.log('Add book response status:', response.status);

        if (response.ok) {
            const result = await response.json();
            console.log('Book added successfully:', result);
            showMessage('bookMessage', 'Book added successfully!', 'success');
            resetForm();

            // Reload page to update list
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        } else {
            const errorMessage = await getErrorMessage(response);
            showMessage('bookMessage', errorMessage, 'error');
        }
    } catch (error) {
        console.error('Error adding book:', error);
        showMessage('bookMessage', 'Error connecting to server: ' + error.message, 'error');
    }
}

// Function to update an existing book
async function updateBook(id, bookData) {
    console.log('Updating book ID:', id, 'with data:', bookData);

    try {
        const response = await fetch(`/Books/Update/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(bookData)
        });

        console.log('Update book response status:', response.status);

        if (response.ok) {
            const result = await response.json();
            console.log('Book updated successfully:', result);
            showMessage('bookMessage', 'Book updated successfully!', 'success');
            resetForm();

            // Reload page to update list
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        } else {
            const errorMessage = await getErrorMessage(response);
            showMessage('bookMessage', errorMessage, 'error');
        }
    } catch (error) {
        console.error('Error updating book:', error);
        showMessage('bookMessage', 'Error connecting to server: ' + error.message, 'error');
    }
}

// Function to get error message from response
async function getErrorMessage(response) {
    let errorMessage = 'Error saving book';

    try {
        const contentType = response.headers.get('content-type');

        if (contentType && contentType.includes('application/json')) {
            const errorData = await response.json();
            console.error('API Error:', errorData);
            errorMessage = errorData.message || JSON.stringify(errorData);
        } else {
            const errorText = await response.text();
            console.error('API Error Text:', errorText);
            errorMessage = errorText || `Error ${response.status}`;
        }
    } catch (e) {
        console.error('Error parsing response:', e);
        errorMessage = `Error ${response.status}`;
    }

    return errorMessage;
}

// Function to edit book (load data into form)
async function editBook(id) {
    console.log('Loading book for edit, ID:', id);

    try {
        const response = await fetch(`/Books/Get/${id}`);

        if (response.ok) {
            const book = await response.json();
            console.log('Book loaded:', book);

            // Fill form with book data
            document.getElementById('editBookId').value = book.id;
            document.getElementById('bookTitle').value = book.title;
            document.getElementById('bookAuthor').value = book.author;
            document.getElementById('bookGenre').value = book.genre;
            document.getElementById('bookPages').value = book.pages;
            document.getElementById('bookType').value = book.type;
            document.getElementById('bookStatus').value = book.status;

            // Update form UI
            document.getElementById('formTitle').textContent = 'Edit Book';
            document.getElementById('btnSubmitBook').textContent = 'Update Book';
            document.getElementById('btnCancelEdit').classList.remove('hidden');

            // Scroll to form
            document.getElementById('formBook').scrollIntoView({ behavior: 'smooth' });
        } else {
            showMessage('bookMessage', 'Error loading book', 'error');
        }
    } catch (error) {
        console.error('Error loading book:', error);
        showMessage('bookMessage', 'Error loading book', 'error');
    }
}

// Function to delete book
async function deleteBook(id) {
    if (!confirm('Are you sure you want to delete this book?')) {
        return;
    }

    console.log('Deleting book ID:', id);

    try {
        const response = await fetch(`/Books/Delete/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            console.log('Book deleted successfully');
            showMessage('bookMessage', 'Book deleted successfully!', 'success');

            // Remove row from table
            const row = document.querySelector(`tr[data-id="${id}"]`);
            if (row) {
                row.remove();
            }

            // If no more books, reload to show empty state
            const tbody = document.getElementById('booksTableBody');
            if (tbody && tbody.children.length === 0) {
                setTimeout(() => {
                    window.location.reload();
                }, 1000);
            }
        } else {
            showMessage('bookMessage', 'Error deleting book', 'error');
        }
    } catch (error) {
        console.error('Error deleting book:', error);
        showMessage('bookMessage', 'Error connecting to server', 'error');
    }
}

// Function to cancel edit
function cancelEdit() {
    resetForm();
    clearMessage('bookMessage');
}

// Function to reset form
function resetForm() {
    document.getElementById('formBook').reset();
    document.getElementById('editBookId').value = '';
    document.getElementById('formTitle').textContent = 'Add Book';
    document.getElementById('btnSubmitBook').textContent = 'Add Book';
    document.getElementById('btnCancelEdit').classList.add('hidden');
}

// Function to show messages
function showMessage(elementId, message, type) {
    const element = document.getElementById(elementId);
    const className = type === 'error' ? 'error-message' : 'success-message';
    element.innerHTML = `<div class="${className}">${message}</div>`;

    setTimeout(() => {
        clearMessage(elementId);
    }, 5000);
}

// Function to clear messages
function clearMessage(elementId) {
    document.getElementById(elementId).innerHTML = '';
}