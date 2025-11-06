// ===== COLLECTIONS PAGE SCRIPT =====

let allBooks = [];
let allAuthors = [];

document.addEventListener('DOMContentLoaded', function () {
    loadCollections();
    loadBooksForSelect();
    loadAuthorsForSelect();
    initializeCollectionForm();
    initializeModals();
});

// ===== SIDEBAR TOGGLE =====
function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');

    if (sidebar && overlay) {
        sidebar.classList.toggle('mobile-visible');
        overlay.classList.toggle('active');
    }
}

function initializeCollectionForm() {
    const form = document.getElementById('formCollection');
    if (form) {
        form.addEventListener('submit', async function(e) {
            e.preventDefault();

            const collectionId = document.getElementById('editCollectionId').value;
            const collectionData = {
                name: document.getElementById('collectionName').value,
                description: document.getElementById('collectionDescription').value || null
            };

            if (collectionId) {
                await updateCollection(collectionId, collectionData);
            } else {
                await addCollection(collectionData);
            }
        });
    }
}

function initializeModals() {
    // Initialize add book modal form
    const formAddBook = document.getElementById('formAddBook');
    if (formAddBook) {
        formAddBook.addEventListener('submit', async function(e) {
            e.preventDefault();
            await handleAddBookFromModal();
        });
    }

    // Initialize add author modal form
    const formAddAuthor = document.getElementById('formAddAuthor');
    if (formAddAuthor) {
        formAddAuthor.addEventListener('submit', async function(e) {
            e.preventDefault();
            await handleAddAuthorFromModal();
        });
    }

    // Close modals when clicking outside
    window.addEventListener('click', function(e) {
        if (e.target.classList.contains('modal')) {
            e.target.style.display = 'none';
        }
    });
}

// ===== LOAD DATA FOR SELECT2 DROPDOWNS =====

async function loadBooksForSelect() {
    try {
        const response = await fetch('/Books/GetAll');
        if (response.ok) {
            allBooks = await response.json();
            initializeBookSelect();
        }
    } catch (error) {
        console.error('Error loading books:', error);
    }
}

async function loadAuthorsForSelect() {
    try {
        const response = await fetch('/Authors/GetAll');
        if (response.ok) {
            allAuthors = await response.json();
            initializeAuthorSelect();
        }
    } catch (error) {
        console.error('Error loading authors:', error);
    }
}

function initializeBookSelect() {
    // Sort books alphabetically by title
    const sortedBooks = [...allBooks].sort((a, b) => a.title.localeCompare(b.title));

    const select = $('#collectionBooks');
    select.empty();

    sortedBooks.forEach(book => {
        const option = new Option(book.title, book.id, false, false);
        select.append(option);
    });

    // Initialize Select2 with search
    select.select2({
        placeholder: 'Select books...',
        allowClear: true,
        width: '100%'
    });
}

function initializeAuthorSelect() {
    // Sort authors alphabetically by name
    const sortedAuthors = [...allAuthors].sort((a, b) => a.name.localeCompare(b.name));

    const select = $('#collectionAuthors');
    select.empty();

    sortedAuthors.forEach(author => {
        const option = new Option(author.name, author.id, false, false);
        select.append(option);
    });

    // Initialize Select2 with search
    select.select2({
        placeholder: 'Select authors...',
        allowClear: true,
        width: '100%'
    });
}

// ===== MODAL FUNCTIONS =====

function openAddBookModal() {
    document.getElementById('addBookModal').style.display = 'block';
}

function closeAddBookModal() {
    document.getElementById('addBookModal').style.display = 'none';
    document.getElementById('formAddBook').reset();
}

function openAddAuthorModal() {
    document.getElementById('addAuthorModal').style.display = 'block';
}

function closeAddAuthorModal() {
    document.getElementById('addAuthorModal').style.display = 'none';
    document.getElementById('formAddAuthor').reset();
}

async function handleAddBookFromModal() {
    const bookData = {
        title: document.getElementById('modalBookTitle').value,
        author: document.getElementById('modalBookAuthor').value || null,
        status: 'not-read'
    };

    try {
        const response = await fetch('/Books/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(bookData)
        });

        if (response.ok) {
            const newBook = await response.json();
            closeAddBookModal();

            // Reload books and reinitialize select
            await loadBooksForSelect();

            // Select the newly added book
            $('#collectionBooks').val([...($('#collectionBooks').val() || []), newBook.id]).trigger('change');

            showMessage('collectionMessage', 'Book added successfully!', 'success');
        } else {
            const errorMessage = await getErrorMessage(response);
            alert('Error adding book: ' + errorMessage);
        }
    } catch (error) {
        console.error('Error adding book:', error);
        alert('Error adding book: ' + error.message);
    }
}

async function handleAddAuthorFromModal() {
    const authorData = {
        name: document.getElementById('modalAuthorName').value,
        nationality: document.getElementById('modalAuthorNationality').value || null,
        bio: document.getElementById('modalAuthorBio').value || null
    };

    try {
        const response = await fetch('/Authors/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(authorData)
        });

        if (response.ok) {
            const newAuthor = await response.json();
            closeAddAuthorModal();

            // Reload authors and reinitialize select
            await loadAuthorsForSelect();

            // Select the newly added author
            $('#collectionAuthors').val([...($('#collectionAuthors').val() || []), newAuthor.id]).trigger('change');

            showMessage('collectionMessage', 'Author added successfully!', 'success');
        } else {
            const errorMessage = await getErrorMessage(response);
            alert('Error adding author: ' + errorMessage);
        }
    } catch (error) {
        console.error('Error adding author:', error);
        alert('Error adding author: ' + error.message);
    }
}

// ===== COLLECTION FORM FUNCTIONS =====

function showCollectionForm() {
    const formSection = document.getElementById('collectionFormSection');
    const tableSection = document.getElementById('collectionsSection');

    if (formSection && tableSection) {
        formSection.classList.remove('hidden');
        tableSection.classList.add('hidden');
    }
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

        // Clear Select2
        $('#collectionBooks').val(null).trigger('change');
        $('#collectionAuthors').val(null).trigger('change');
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

function calculateCollectionStatus(books) {
    if (!books || books.length === 0) {
        return 'not-read';
    }

    const allRead = books.every(book => book.status === 'read');
    const anyInProgress = books.some(book => book.status === 'reading' || book.status === 'read');

    if (allRead) {
        return 'read';
    } else if (anyInProgress) {
        return 'in-progress';
    } else {
        return 'not-read';
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
        const status = calculateCollectionStatus(collection.books);
        const statusClass = status === 'read' ? 'success' : status === 'in-progress' ? 'warning' : 'info';
        const statusLabel = status === 'read' ? 'Completed' : status === 'in-progress' ? 'In Progress' : 'Not Started';

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
                    <span class="status-badge status-${statusClass}">${statusLabel}</span>
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

            // Set selected books in Select2
            if (collection.books && collection.books.length > 0) {
                const bookIds = collection.books.map(b => b.id);
                $('#collectionBooks').val(bookIds).trigger('change');
            }

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
        showMessage('collectionMessage', 'Error deleting collection', 'error');
    }
}

// ===== UTILITY FUNCTIONS =====

function showMessage(elementId, message, type) {
    const messageDiv = document.getElementById(elementId);
    if (!messageDiv) return;

    messageDiv.className = `message message-${type}`;
    messageDiv.textContent = message;
    messageDiv.style.display = 'block';

    setTimeout(() => {
        messageDiv.style.display = 'none';
    }, 5000);
}

function clearMessage(elementId) {
    const messageDiv = document.getElementById(elementId);
    if (messageDiv) {
        messageDiv.style.display = 'none';
        messageDiv.textContent = '';
    }
}

async function getErrorMessage(response) {
    try {
        const data = await response.json();
        return data.message || 'An error occurred';
    } catch {
        return 'An error occurred';
    }
}
