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

// ===== SIDEBAR TOGGLE =====
function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');

    if (sidebar && overlay) {
        sidebar.classList.toggle('mobile-visible');
        overlay.classList.toggle('active');
    }
}

// ===== NAVIGATION =====
function initializeNavigation() {
    const navBooks = document.getElementById('nav-books');
    const navAuthors = document.getElementById('nav-authors');
    const navCollections = document.getElementById('nav-collections');
    const navStatistics = document.getElementById('nav-statistics');
    const navSettings = document.getElementById('nav-settings');

    if (navBooks) {
        navBooks.addEventListener('click', function(e) {
            e.preventDefault();
            showSection('books');
            setActiveNav(this);
        });
    }

    if (navAuthors) {
        navAuthors.addEventListener('click', function(e) {
            e.preventDefault();
            showSection('authors');
            setActiveNav(this);
        });
    }

    if (navCollections) {
        navCollections.addEventListener('click', function(e) {
            e.preventDefault();
            showSection('collections');
            setActiveNav(this);
        });
    }

    if (navStatistics) {
        navStatistics.addEventListener('click', function(e) {
            e.preventDefault();
            showSection('statistics');
            setActiveNav(this);
        });
    }

    if (navSettings) {
        navSettings.addEventListener('click', function(e) {
            e.preventDefault();
            showSection('settings');
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

function showSection(sectionName) {
    // Hide all sections
    document.getElementById('bookFormSection')?.classList.add('hidden');
    document.getElementById('booksTableSection')?.classList.add('hidden');
    document.getElementById('authorFormSection')?.classList.add('hidden');
    document.getElementById('authorsSection')?.classList.add('hidden');
    document.getElementById('collectionFormSection')?.classList.add('hidden');
    document.getElementById('collectionsSection')?.classList.add('hidden');
    document.getElementById('statisticsSection')?.classList.add('hidden');
    document.getElementById('settingsSection')?.classList.add('hidden');

    // Show requested section and load data
    switch(sectionName) {
        case 'books':
            document.getElementById('booksTableSection')?.classList.remove('hidden');
            break;
        case 'authors':
            document.getElementById('authorsSection')?.classList.remove('hidden');
            loadAuthors();
            break;
        case 'collections':
            document.getElementById('collectionsSection')?.classList.remove('hidden');
            loadCollections();
            break;
        case 'statistics':
            document.getElementById('statisticsSection')?.classList.remove('hidden');
            break;
        case 'settings':
            document.getElementById('settingsSection')?.classList.remove('hidden');
            break;
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
    showSection('books');
    const formSection = document.getElementById('bookFormSection');
    const tableSection = document.getElementById('booksTableSection');

    if (formSection && tableSection) {
        formSection.classList.remove('hidden');
        tableSection.classList.add('hidden');
    }

    // Set active nav
    document.querySelectorAll('.nav-item').forEach(item => item.classList.remove('active'));
    document.getElementById('nav-books')?.classList.add('active');
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
    showSection('books');
}

// ===== AUTHORS SECTION =====

function showAuthorForm() {
    showSection('authors');
    const formSection = document.getElementById('authorFormSection');
    const tableSection = document.getElementById('authorsSection');

    if (formSection && tableSection) {
        formSection.classList.remove('hidden');
        tableSection.classList.add('hidden');
    }

    // Set active nav
    document.querySelectorAll('.nav-item').forEach(item => item.classList.remove('active'));
    document.getElementById('nav-authors')?.classList.add('active');
}

function hideAuthorForm() {
    const formSection = document.getElementById('authorFormSection');
    const tableSection = document.getElementById('authorsSection');

    if (formSection && tableSection) {
        formSection.classList.add('hidden');
        tableSection.classList.remove('hidden');
        resetAuthorForm();
        clearMessage('authorMessage');
    }
}

function cancelAuthorEdit() {
    hideAuthorForm();
}

function resetAuthorForm() {
    const form = document.getElementById('formAuthor');
    if (form) {
        form.reset();
        document.getElementById('editAuthorId').value = '';
        document.getElementById('authorFormTitle').textContent = 'Add New Author';
        const submitBtn = document.getElementById('btnSubmitAuthor');
        submitBtn.innerHTML = '<i class="fas fa-save"></i> Save Author';
    }
}

async function loadAuthors() {
    try {
        const response = await fetch('/Authors/GetAll');
        if (response.ok) {
            const authors = await response.json();
            displayAuthors(authors);
        }
    } catch (error) {
        console.error('Error loading authors:', error);
    }
}

function displayAuthors(authors) {
    const container = document.getElementById('authorsTableContainer');

    if (!authors || authors.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <i class="fas fa-user-edit empty-icon"></i>
                <h3>No authors yet</h3>
                <p>Start adding authors to your library!</p>
                <button class="btn-primary" onclick="showAuthorForm()">
                    <i class="fas fa-plus"></i> Add Your First Author
                </button>
            </div>
        `;
        return;
    }

    let tableHtml = `
        <table class="modern-table">
            <thead>
                <tr>
                    <th>NAME</th>
                    <th>NATIONALITY</th>
                    <th>BOOKS</th>
                    <th>ACTIONS</th>
                </tr>
            </thead>
            <tbody>
    `;

    authors.forEach(author => {
        tableHtml += `
            <tr data-id="${author.id}">
                <td class="book-title">${author.name}</td>
                <td>${author.nationality || '-'}</td>
                <td>${author.bookCount || 0}</td>
                <td class="actions">
                    <button class="btn-icon btn-icon-edit" onclick="editAuthor(${author.id})" title="Edit author">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn-icon btn-icon-delete" onclick="deleteAuthor(${author.id})" title="Delete author">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            </tr>
        `;
    });

    tableHtml += `
            </tbody>
        </table>
    `;

    container.innerHTML = tableHtml;
}

async function addAuthor(authorData) {
    try {
        const response = await fetch('/Authors/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(authorData)
        });

        if (response.ok) {
            const result = await response.json();
            showMessage('authorMessage', 'Author added successfully!', 'success');
            resetAuthorForm();

            setTimeout(() => {
                hideAuthorForm();
                loadAuthors();
            }, 1000);
        } else {
            const errorMessage = await getErrorMessage(response);
            showMessage('authorMessage', errorMessage, 'error');
        }
    } catch (error) {
        console.error('Error adding author:', error);
        showMessage('authorMessage', 'Error connecting to server: ' + error.message, 'error');
    }
}

async function updateAuthor(id, authorData) {
    try {
        const response = await fetch(`/Authors/Update/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(authorData)
        });

        if (response.ok) {
            const result = await response.json();
            showMessage('authorMessage', 'Author updated successfully!', 'success');
            resetAuthorForm();

            setTimeout(() => {
                hideAuthorForm();
                loadAuthors();
            }, 1000);
        } else {
            const errorMessage = await getErrorMessage(response);
            showMessage('authorMessage', errorMessage, 'error');
        }
    } catch (error) {
        console.error('Error updating author:', error);
        showMessage('authorMessage', 'Error connecting to server: ' + error.message, 'error');
    }
}

async function editAuthor(id) {
    try {
        const response = await fetch(`/Authors/Get/${id}`);

        if (response.ok) {
            const author = await response.json();

            document.getElementById('editAuthorId').value = author.id;
            document.getElementById('authorName').value = author.name;
            document.getElementById('authorNationality').value = author.nationality || '';
            document.getElementById('authorBio').value = author.bio || '';

            document.getElementById('authorFormTitle').textContent = 'Edit Author';
            const submitBtn = document.getElementById('btnSubmitAuthor');
            submitBtn.innerHTML = '<i class="fas fa-save"></i> Update Author';

            showAuthorForm();
        } else {
            showMessage('authorMessage', 'Error loading author', 'error');
        }
    } catch (error) {
        console.error('Error loading author:', error);
        showMessage('authorMessage', 'Error loading author', 'error');
    }
}

async function deleteAuthor(id) {
    if (!confirm('Are you sure you want to delete this author?\\n\\nThis action cannot be undone.')) {
        return;
    }

    try {
        const response = await fetch(`/Authors/Delete/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            showMessage('authorMessage', 'Author deleted successfully!', 'success');

            const row = document.querySelector(`#authorsTableContainer tr[data-id="${id}"]`);
            if (row) {
                row.style.opacity = '0';
                setTimeout(() => row.remove(), 300);
            }

            setTimeout(() => {
                loadAuthors();
            }, 1000);
        } else {
            showMessage('authorMessage', 'Error deleting author', 'error');
        }
    } catch (error) {
        console.error('Error deleting author:', error);
        showMessage('authorMessage', 'Error connecting to server', 'error');
    }
}

// ===== COLLECTIONS SECTION =====

function showCollectionForm() {
    showSection('collections');
    const formSection = document.getElementById('collectionFormSection');
    const tableSection = document.getElementById('collectionsSection');

    if (formSection && tableSection) {
        formSection.classList.remove('hidden');
        tableSection.classList.add('hidden');
    }

    // Set active nav
    document.querySelectorAll('.nav-item').forEach(item => item.classList.remove('active'));
    document.getElementById('nav-collections')?.classList.add('active');
}

function hideCollectionForm() {
    const formSection = document.getElementById('collectionFormSection');
    const tableSection = document.getElementById('collectionsSection');

    if (formSection && tableSection) {
        formSection.classList.add('hidden');
        tableSection.classList.remove('hidden');
        resetCollectionForm();
        clearMessage('collectionMessage');
    }
}

function cancelCollectionEdit() {
    hideCollectionForm();
}

function resetCollectionForm() {
    const form = document.getElementById('formCollection');
    if (form) {
        form.reset();
        document.getElementById('editCollectionId').value = '';
        document.getElementById('collectionFormTitle').textContent = 'Create New Collection';
        const submitBtn = document.getElementById('btnSubmitCollection');
        submitBtn.innerHTML = '<i class="fas fa-save"></i> Save Collection';
    }
}

async function loadCollections() {
    try {
        const response = await fetch('/Collections/GetAll');
        if (response.ok) {
            const collections = await response.json();
            displayCollections(collections);
        }
    } catch (error) {
        console.error('Error loading collections:', error);
    }
}

function displayCollections(collections) {
    const container = document.getElementById('collectionsContainer');

    if (!collections || collections.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <i class="fas fa-layer-group empty-icon"></i>
                <h3>No collections yet</h3>
                <p>Create collections to organize your books by series, genre, or theme!</p>
                <button class="btn-primary" onclick="showCollectionForm()">
                    <i class="fas fa-plus"></i> Create Your First Collection
                </button>
            </div>
        `;
        return;
    }

    let cardsHtml = '';

    collections.forEach(collection => {
        cardsHtml += `
            <div class="collection-card" data-id="${collection.id}">
                <div class="collection-header">
                    <h3>${collection.name}</h3>
                    <div class="collection-actions">
                        <button class="btn-icon btn-icon-edit" onclick="editCollection(${collection.id})" title="Edit collection">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn-icon btn-icon-delete" onclick="deleteCollection(${collection.id})" title="Delete collection">
                            <i class="fas fa-trash"></i>
                        </button>
                    </div>
                </div>
                <p class="collection-description">${collection.description || 'No description'}</p>
                <div class="collection-stats">
                    <span><i class="fas fa-book"></i> ${collection.bookCount || 0} books</span>
                </div>
            </div>
        `;
    });

    container.innerHTML = cardsHtml;
}

async function addCollection(collectionData) {
    try {
        const response = await fetch('/Collections/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(collectionData)
        });

        if (response.ok) {
            const result = await response.json();
            showMessage('collectionMessage', 'Collection created successfully!', 'success');
            resetCollectionForm();

            setTimeout(() => {
                hideCollectionForm();
                loadCollections();
            }, 1000);
        } else {
            const errorMessage = await getErrorMessage(response);
            showMessage('collectionMessage', errorMessage, 'error');
        }
    } catch (error) {
        console.error('Error creating collection:', error);
        showMessage('collectionMessage', 'Error connecting to server: ' + error.message, 'error');
    }
}

async function updateCollection(id, collectionData) {
    try {
        const response = await fetch(`/Collections/Update/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(collectionData)
        });

        if (response.ok) {
            const result = await response.json();
            showMessage('collectionMessage', 'Collection updated successfully!', 'success');
            resetCollectionForm();

            setTimeout(() => {
                hideCollectionForm();
                loadCollections();
            }, 1000);
        } else {
            const errorMessage = await getErrorMessage(response);
            showMessage('collectionMessage', errorMessage, 'error');
        }
    } catch (error) {
        console.error('Error updating collection:', error);
        showMessage('collectionMessage', 'Error connecting to server: ' + error.message, 'error');
    }
}

async function editCollection(id) {
    try {
        const response = await fetch(`/Collections/Get/${id}`);

        if (response.ok) {
            const collection = await response.json();

            document.getElementById('editCollectionId').value = collection.id;
            document.getElementById('collectionName').value = collection.name;
            document.getElementById('collectionDescription').value = collection.description || '';

            document.getElementById('collectionFormTitle').textContent = 'Edit Collection';
            const submitBtn = document.getElementById('btnSubmitCollection');
            submitBtn.innerHTML = '<i class="fas fa-save"></i> Update Collection';

            showCollectionForm();
        } else {
            showMessage('collectionMessage', 'Error loading collection', 'error');
        }
    } catch (error) {
        console.error('Error loading collection:', error);
        showMessage('collectionMessage', 'Error loading collection', 'error');
    }
}

async function deleteCollection(id) {
    if (!confirm('Are you sure you want to delete this collection?\\n\\nThis action cannot be undone.')) {
        return;
    }

    try {
        const response = await fetch(`/Collections/Delete/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            showMessage('collectionMessage', 'Collection deleted successfully!', 'success');

            const card = document.querySelector(`.collection-card[data-id="${id}"]`);
            if (card) {
                card.style.opacity = '0';
                setTimeout(() => card.remove(), 300);
            }

            setTimeout(() => {
                loadCollections();
            }, 1000);
        } else {
            showMessage('collectionMessage', 'Error deleting collection', 'error');
        }
    } catch (error) {
        console.error('Error deleting collection:', error);
        showMessage('collectionMessage', 'Error connecting to server', 'error');
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

// Author form submission
const formAuthor = document.getElementById('formAuthor');
if (formAuthor) {
    formAuthor.addEventListener('submit', async function (e) {
        e.preventDefault();

        const editId = document.getElementById('editAuthorId').value;
        const authorData = {
            name: document.getElementById('authorName').value.trim(),
            nationality: document.getElementById('authorNationality').value.trim() || null,
            bio: document.getElementById('authorBio').value.trim() || null
        };

        if (!authorData.name) {
            showMessage('authorMessage', 'Please fill in author name', 'error');
            return;
        }

        if (editId) {
            await updateAuthor(editId, authorData);
        } else {
            await addAuthor(authorData);
        }
    });
}

// Collection form submission
const formCollection = document.getElementById('formCollection');
if (formCollection) {
    formCollection.addEventListener('submit', async function (e) {
        e.preventDefault();

        const editId = document.getElementById('editCollectionId').value;
        const collectionData = {
            name: document.getElementById('collectionName').value.trim(),
            description: document.getElementById('collectionDescription').value.trim() || null
        };

        if (!collectionData.name) {
            showMessage('collectionMessage', 'Please fill in collection name', 'error');
            return;
        }

        if (editId) {
            await updateCollection(editId, collectionData);
        } else {
            await addCollection(collectionData);
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
