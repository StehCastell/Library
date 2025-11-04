// ===== THEME MANAGEMENT =====
document.addEventListener('DOMContentLoaded', function () {
    // Force light theme as default (remove old saved theme)
    localStorage.removeItem('theme');
    const savedTheme = localStorage.getItem('theme') || 'light';
    applyTheme(savedTheme);

    // Initialize search functionality
    initializeSearch();

    // Initialize navigation
    initializeNavigation();

    // Update stats
    updateStats();
});

function changeTheme() {
    const currentTheme = document.body.classList.contains('dark-theme') ? 'light' : 'dark';
    applyTheme(currentTheme);
    localStorage.setItem('theme', currentTheme);
}

function applyTheme(theme) {
    const themeIcon = document.querySelector('.theme-toggle-btn i');
    if (theme === 'dark') {
        document.body.classList.add('dark-theme');
        if (themeIcon) themeIcon.className = 'fas fa-sun';
    } else {
        document.body.classList.remove('dark-theme');
        if (themeIcon) themeIcon.className = 'fas fa-moon';
    }
}

// ===== NAVIGATION =====
function initializeNavigation() {
    const navBooks = document.getElementById('nav-books');
    const navAddBook = document.getElementById('nav-add-book');

    if (navBooks) {
        navBooks.addEventListener('click', function(e) {
            e.preventDefault();
            showBooksTable();
            setActiveNav(this);
        });
    }

    if (navAddBook) {
        navAddBook.addEventListener('click', function(e) {
            e.preventDefault();
            showBookForm();
            setActiveNav(this);
        });
    }
}

function setActiveNav(activeItem) {
    document.querySelectorAll('.nav-item').forEach(item => {
        item.classList.remove('active');
    });
    if (activeItem) {
        activeItem.classList.add('active');
    }
}

// ===== SEARCH FUNCTIONALITY =====
function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', function() {
            filterBooks(this.value.toLowerCase());
        });
    }
}

function filterBooks(searchTerm) {
    const rows = document.querySelectorAll('.modern-table tbody tr');
    let visibleCount = 0;

    rows.forEach(row => {
        const text = row.textContent.toLowerCase();
        if (text.includes(searchTerm)) {
            row.style.display = '';
            visibleCount++;
        } else {
            row.style.display = 'none';
        }
    });

    // Show/hide empty state
    const tableContainer = document.getElementById('booksTableContainer');
    if (visibleCount === 0 && searchTerm !== '') {
        if (!document.querySelector('.search-empty-state')) {
            const emptyState = document.createElement('div');
            emptyState.className = 'empty-state search-empty-state';
            emptyState.innerHTML = `
                <i class="fas fa-search empty-icon"></i>
                <h3>No books found</h3>
                <p>Try searching with different keywords</p>
            `;
            tableContainer.appendChild(emptyState);
        }
        document.querySelector('.modern-table')?.style.setProperty('display', 'none');
    } else {
        document.querySelector('.search-empty-state')?.remove();
        document.querySelector('.modern-table')?.style.removeProperty('display');
    }
}

// ===== STATS UPDATE =====
function updateStats() {
    const rows = document.querySelectorAll('.modern-table tbody tr');
    const totalBooks = rows.length;
    let booksRead = 0;
    let booksReading = 0;
    let booksNotRead = 0;

    rows.forEach(row => {
        const statusBadge = row.querySelector('.badge-status');
        if (statusBadge) {
            if (statusBadge.classList.contains('status-read')) booksRead++;
            else if (statusBadge.classList.contains('status-reading')) booksReading++;
            else if (statusBadge.classList.contains('status-not-read')) booksNotRead++;
        }
    });

    const totalBooksEl = document.getElementById('totalBooks');
    const booksReadEl = document.getElementById('booksRead');
    const booksReadingEl = document.getElementById('booksReading');
    const booksNotReadEl = document.getElementById('booksNotRead');

    if (totalBooksEl) totalBooksEl.textContent = totalBooks;
    if (booksReadEl) booksReadEl.textContent = booksRead;
    if (booksReadingEl) booksReadingEl.textContent = booksReading;
    if (booksNotReadEl) booksNotReadEl.textContent = booksNotRead;
}

// ===== FORM MANAGEMENT =====
function showBookForm() {
    const formSection = document.getElementById('bookFormSection');
    const tableSection = document.getElementById('booksTableSection');

    if (formSection && tableSection) {
        formSection.classList.remove('hidden');
        tableSection.classList.add('hidden');
    }
}

function hideBookForm() {
    const formSection = document.getElementById('bookFormSection');
    const tableSection = document.getElementById('booksTableSection');

    if (formSection && tableSection) {
        formSection.classList.add('hidden');
        tableSection.classList.remove('hidden');
        resetForm();
        clearMessage('bookMessage');
    }
}

function showBooksTable() {
    const formSection = document.getElementById('bookFormSection');
    const tableSection = document.getElementById('booksTableSection');

    if (formSection && tableSection) {
        formSection.classList.add('hidden');
        tableSection.classList.remove('hidden');
    }
}

// ===== FORM SUBMISSION =====
const formBook = document.getElementById('formBook');
if (formBook) {
    formBook.addEventListener('submit', async function (e) {
        e.preventDefault();

        const editId = document.getElementById('editBookId').value;
        const bookData = getFormData();

        if (!validateBookData(bookData)) {
            showMessage('bookMessage', 'Please fill in all fields correctly', 'error');
            return;
        }

        if (editId) {
            await updateBook(editId, bookData);
        } else {
            await addBook(bookData);
        }
    });
}

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

function validateBookData(book) {
    return book.title &&
        book.author &&
        book.genre &&
        book.pages > 0 &&
        book.type &&
        book.status;
}

// ===== API FUNCTIONS =====
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

async function editBook(id) {
    console.log('Loading book for edit, ID:', id);

    try {
        const response = await fetch(`/Books/Get/${id}`);

        if (response.ok) {
            const book = await response.json();
            console.log('Book loaded:', book);

            document.getElementById('editBookId').value = book.id;
            document.getElementById('bookTitle').value = book.title;
            document.getElementById('bookAuthor').value = book.author;
            document.getElementById('bookGenre').value = book.genre;
            document.getElementById('bookPages').value = book.pages;
            document.getElementById('bookType').value = book.type;
            document.getElementById('bookStatus').value = book.status;

            document.getElementById('formTitle').textContent = 'Edit Book';
            const submitBtn = document.getElementById('btnSubmitBook');
            submitBtn.innerHTML = '<i class="fas fa-save"></i> Update Book';

            showBookForm();

            setTimeout(() => {
                document.getElementById('bookFormSection').scrollIntoView({ behavior: 'smooth' });
            }, 100);
        } else {
            showMessage('bookMessage', 'Error loading book', 'error');
        }
    } catch (error) {
        console.error('Error loading book:', error);
        showMessage('bookMessage', 'Error loading book', 'error');
    }
}

async function deleteBook(id) {
    if (!confirm('Are you sure you want to delete this book?\n\nThis action cannot be undone.')) {
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

            const row = document.querySelector(`tr[data-id="${id}"]`);
            if (row) {
                row.style.opacity = '0';
                setTimeout(() => row.remove(), 300);
            }

            setTimeout(() => {
                window.location.reload();
            }, 1000);
        } else {
            showMessage('bookMessage', 'Error deleting book', 'error');
        }
    } catch (error) {
        console.error('Error deleting book:', error);
        showMessage('bookMessage', 'Error connecting to server', 'error');
    }
}

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

// ===== UTILITY FUNCTIONS =====
function cancelEdit() {
    hideBookForm();
}

function resetForm() {
    const form = document.getElementById('formBook');
    if (form) {
        form.reset();
        document.getElementById('editBookId').value = '';
        document.getElementById('formTitle').textContent = 'Add New Book';
        const submitBtn = document.getElementById('btnSubmitBook');
        submitBtn.innerHTML = '<i class="fas fa-save"></i> Save Book';
    }
}

function showMessage(elementId, message, type) {
    const element = document.getElementById(elementId);
    if (!element) return;

    const className = type === 'error' ? 'error-message' : 'success-message';
    const icon = type === 'error' ? '<i class="fas fa-exclamation-circle"></i>' : '<i class="fas fa-check-circle"></i>';
    element.innerHTML = `<div class="${className}">${icon} ${message}</div>`;

    setTimeout(() => {
        clearMessage(elementId);
    }, 5000);
}

function clearMessage(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.innerHTML = '';
    }
}
