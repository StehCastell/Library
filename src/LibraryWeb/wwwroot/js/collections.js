// ===== COLLECTIONS PAGE SCRIPT =====

let allBooks = [];
let allAuthors = [];

document.addEventListener('DOMContentLoaded', function () {
    // Apply saved theme
    const savedTheme = localStorage.getItem('theme') || 'light';
    applyTheme(savedTheme);

    loadCollections();
    loadBooksForSelect();
    loadAuthorsForSelect();
    initializeCollectionForm();
    initializeModals();
});

// ===== THEME MANAGEMENT =====
async function changeTheme() {
    const currentTheme = document.body.classList.contains('dark-theme') ? 'light' : 'dark';
    applyTheme(currentTheme);
    localStorage.setItem('theme', currentTheme);

    // Save to database
    try {
        const response = await fetch('/books/UpdateTheme', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ theme: currentTheme })
        });

        if (!response.ok) {
            console.error('Failed to save theme to database');
        }
    } catch (error) {
        console.error('Error saving theme:', error);
    }
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

function initializeCollectionForm() {
    const form = document.getElementById('formCollection');
    if (form) {
        form.addEventListener('submit', async function(e) {
            e.preventDefault();

            const collectionId = document.getElementById('editCollectionId').value;
            const collectionData = {
                name: document.getElementById('collectionName').value,
                description: document.getElementById('collectionDescription').value || null,
                profileImage: currentCollectionProfileImageBase64
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
        const response = await fetch('/books/getall');
        if (response.ok) {
            allBooks = await response.json();
            console.log('Books loaded:', allBooks.length, allBooks);
            initializeBookSelect();
        } else {
            console.error('Failed to load books:', response.status, response.statusText);
        }
    } catch (error) {
        console.error('Error loading books:', error);
    }
}

async function loadAuthorsForSelect() {
    try {
        const response = await fetch('/authors/getall');
        if (response.ok) {
            allAuthors = await response.json();
            initializeAuthorSelect();
        }
    } catch (error) {
        console.error('Error loading authors:', error);
    }
}

function initializeBookSelect() {
    console.log('Initializing book select, total books:', allBooks.length);

    // Sort books alphabetically by title
    const sortedBooks = [...allBooks].sort((a, b) => a.title.localeCompare(b.title));

    const select = $('#collectionBooks');
    console.log('Select element found:', select.length > 0);

    // Destroy existing Select2 if present
    if (select.hasClass("select2-hidden-accessible")) {
        console.log('Destroying existing Select2');
        select.select2('destroy');
    }

    select.empty();

    sortedBooks.forEach(book => {
        const option = new Option(book.title, book.id, false, false);
        select.append(option);
    });

    console.log('Options added to select:', select.find('option').length);

    // Initialize Select2 with search
    try {
        select.select2({
            placeholder: 'Select books...',
            allowClear: true,
            width: '100%'
        });
        console.log('Select2 initialized successfully');

        // Add event listener to update badges when selection changes
        select.on('change', function() {
            updateBookBadges();
        });
    } catch (error) {
        console.error('Error initializing Select2:', error);
    }
}

function initializeAuthorSelect() {
    // Sort authors alphabetically by name
    const sortedAuthors = [...allAuthors].sort((a, b) => a.name.localeCompare(b.name));

    const select = $('#collectionAuthors');

    // Destroy existing Select2 if present
    if (select.hasClass("select2-hidden-accessible")) {
        select.select2('destroy');
    }

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

    // Add event listener to update badges when selection changes
    select.on('change', function() {
        updateCollectionAuthorBadges();
    });
}

// ===== BADGE FUNCTIONS =====

function updateCollectionAuthorBadges() {
    const selectedIds = $('#collectionAuthors').val() || [];
    const badgesContainer = document.getElementById('selectedCollectionAuthorsBadges');

    if (!badgesContainer) return;

    badgesContainer.innerHTML = '';

    selectedIds.forEach(authorId => {
        const author = allAuthors.find(a => a.id == authorId);
        if (author) {
            const badge = document.createElement('div');
            badge.className = 'author-badge';
            badge.innerHTML = `
                <span>${author.name}</span>
                <span class="author-badge-remove" onclick="removeCollectionAuthorBadge(${authorId})">×</span>
            `;
            badgesContainer.appendChild(badge);
        }
    });
}

function removeCollectionAuthorBadge(authorId) {
    const select = $('#collectionAuthors');
    const selectedValues = select.val() || [];
    const newValues = selectedValues.filter(id => id != authorId);
    select.val(newValues).trigger('change');
}

function updateBookBadges() {
    const selectedIds = $('#collectionBooks').val() || [];
    const badgesContainer = document.getElementById('selectedCollectionBooksBadges');

    if (!badgesContainer) return;

    badgesContainer.innerHTML = '';

    selectedIds.forEach(bookId => {
        const book = allBooks.find(b => b.id == bookId);
        if (book) {
            const badge = document.createElement('div');
            badge.className = 'book-badge';
            badge.innerHTML = `
                <span>${book.title}</span>
                <span class="book-badge-remove" onclick="removeBookBadge(${bookId})">×</span>
            `;
            badgesContainer.appendChild(badge);
        }
    });
}

function removeBookBadge(bookId) {
    const select = $('#collectionBooks');
    const selectedValues = select.val() || [];
    const newValues = selectedValues.filter(id => id != bookId);
    select.val(newValues).trigger('change');
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
        author: document.getElementById('modalBookAuthor').value,
        genre: document.getElementById('modalBookGenre').value,
        pages: parseInt(document.getElementById('modalBookPages').value),
        type: document.getElementById('modalBookType').value,
        status: document.getElementById('modalBookStatus').value
    };

    try {
        const response = await fetch('/books/create', {
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
        const response = await fetch('/authors/create', {
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

        // Clear image
        removeCollectionImage();
    }
}

async function loadCollections() {
    try {
        const response = await fetch('/collections/getall');
        if (response.ok) {
            const collections = await response.json();
            console.log('Collections received:', collections);
            displayCollections(collections);
        }
    } catch (error) {
        console.error('Error loading collections:', error);
    }
}

function calculateCollectionStatus(books) {
    // Se não houver livros, status é não iniciado
    if (!books || books.length === 0) {
        return 'not-started';
    }

    // Contar status dos livros
    const totalBooks = books.length;
    const completedBooks = books.filter(book => book.status === 'read').length;
    const readingBooks = books.filter(book => book.status === 'reading').length;

    // Se TODOS os livros estão concluídos
    if (completedBooks === totalBooks) {
        return 'completed';
    }

    // Se nenhum livro está concluído ou sendo lido
    if (completedBooks === 0 && readingBooks === 0) {
        return 'not-started';
    }

    // Se tem pelo menos 1 livro em andamento OU pelo menos 1 concluído (mas não todos)
    if (readingBooks > 0 || (completedBooks > 0 && completedBooks < totalBooks)) {
        return 'in-progress';
    }

    // Fallback
    return 'not-started';
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
        console.log('Collection:', collection.name, 'Authors:', collection.authors);
        const status = calculateCollectionStatus(collection.books);

        // Determinar classe e label baseado no status
        let statusClass, statusLabel, statusIcon;
        if (status === 'completed') {
            statusClass = 'success';
            statusLabel = 'Reading Completed';
            statusIcon = 'fa-check-circle';
        } else if (status === 'in-progress') {
            statusClass = 'warning';
            statusLabel = 'Reading in Progress';
            statusIcon = 'fa-book-reader';
        } else {
            statusClass = 'info';
            statusLabel = 'Not Started';
            statusIcon = 'fa-bookmark';
        }

        // Build authors list
        let authorsHtml = '';
        if (collection.authors && collection.authors.length > 0) {
            const authorNames = collection.authors.map(author => author.name).join(', ');
            authorsHtml = `
                <div class="collection-authors">
                    <i class="fas fa-user-pen"></i>
                    <span>${authorNames}</span>
                </div>
            `;
        }

        // Calcular progresso de leitura
        const totalBooks = collection.books ? collection.books.length : 0;
        const completedBooks = collection.books ? collection.books.filter(b => b.status === 'read').length : 0;
        const booksProgressText = totalBooks > 0 ? `${completedBooks}/${totalBooks} books read` : 'No books';

        // Profile image HTML
        let profileImageHtml = '';
        if (collection.profileImage) {
            profileImageHtml = `
                <div class="collection-profile-image">
                    <img src="${collection.profileImage}" alt="${collection.name}" class="author-profile-thumbnail">
                </div>
            `;
        } else {
            profileImageHtml = `
                <div class="collection-profile-image">
                    <div class="author-profile-placeholder">
                        <i class="fas fa-layer-group"></i>
                    </div>
                </div>
            `;
        }

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
                ${profileImageHtml}
                <p class="collection-description">${collection.description || 'No description'}</p>
                ${authorsHtml}
                <div class="collection-stats">
                    <span class="status-badge status-${statusClass}">
                        <i class="fas ${statusIcon}"></i> ${statusLabel}
                    </span>
                    <span class="books-count">
                        <i class="fas fa-book"></i> ${booksProgressText}
                    </span>
                </div>
            </div>
        `;
    });

    container.innerHTML = cardsHtml;
}

async function addCollection(collectionData) {
    try {
        const response = await fetch('/collections/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(collectionData)
        });

        if (response.ok) {
            const result = await response.json();

            // Get selected authors and books
            const selectedAuthors = $('#collectionAuthors').val() || [];
            const selectedBooks = $('#collectionBooks').val() || [];

            // Add authors to the collection
            for (const authorId of selectedAuthors) {
                console.log(`Adding author ${authorId} to collection ${result.id}`);
                const response = await fetch(`/collections/addauthor/${result.id}/${authorId}`, {
                    method: 'POST'
                });
                console.log(`Add author response:`, response.status, response.statusText);
            }

            // Add books to the collection
            for (const bookId of selectedBooks) {
                console.log(`Adding book ${bookId} to collection ${result.id}`);
                const response = await fetch(`/collections/addbook/${result.id}/${bookId}`, {
                    method: 'POST'
                });
                console.log(`Add book response:`, response.status, response.statusText);

                if (!response.ok) {
                    const errorData = await response.json().catch(() => ({ message: 'Unknown error' }));
                    console.error(`Failed to add book ${bookId}:`, errorData);
                }
            }

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
        const response = await fetch(`/collections/update/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(collectionData)
        });

        if (response.ok) {
            const result = await response.json();

            // Get selected authors and books from form
            const selectedAuthors = $('#collectionAuthors').val() || [];
            const selectedBooks = $('#collectionBooks').val() || [];

            // Get current collection to compare
            const currentCollectionResponse = await fetch(`/collections/get/${id}`);
            const currentCollection = await currentCollectionResponse.json();

            const currentAuthors = (currentCollection.authors || []).map(a => a.authorId);
            const currentBooks = (currentCollection.books || []).map(b => b.bookId);

            // Remove authors that are no longer selected
            for (const authorId of currentAuthors) {
                if (!selectedAuthors.includes(String(authorId))) {
                    await fetch(`/collections/removeauthor/${id}/${authorId}`, {
                        method: 'DELETE'
                    });
                }
            }

            // Add new authors
            for (const authorId of selectedAuthors) {
                if (!currentAuthors.includes(Number(authorId))) {
                    await fetch(`/collections/addauthor/${id}/${authorId}`, {
                        method: 'POST'
                    });
                }
            }

            // Remove books that are no longer selected
            for (const bookId of currentBooks) {
                if (!selectedBooks.includes(String(bookId))) {
                    await fetch(`/collections/removebook/${id}/${bookId}`, {
                        method: 'DELETE'
                    });
                }
            }

            // Add new books
            for (const bookId of selectedBooks) {
                if (!currentBooks.includes(Number(bookId))) {
                    console.log(`Adding new book ${bookId} to collection ${id}`);
                    const response = await fetch(`/collections/addbook/${id}/${bookId}`, {
                        method: 'POST'
                    });
                    console.log(`Add book response:`, response.status, response.statusText);

                    if (!response.ok) {
                        const errorData = await response.json().catch(() => ({ message: 'Unknown error' }));
                        console.error(`Failed to add book ${bookId}:`, errorData);
                    }
                }
            }

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
        const response = await fetch(`/collections/get/${id}`);

        if (response.ok) {
            const collection = await response.json();

            document.getElementById('editCollectionId').value = collection.id;
            document.getElementById('collectionName').value = collection.name;
            document.getElementById('collectionDescription').value = collection.description || '';

            // Load profile image if exists
            if (collection.profileImage) {
                currentCollectionProfileImageBase64 = collection.profileImage;
                const preview = document.getElementById('collectionImagePreview');
                const previewContainer = document.getElementById('collectionImagePreviewContainer');
                const placeholder = document.getElementById('collectionImageUploadPlaceholder');

                if (preview && previewContainer && placeholder) {
                    preview.src = collection.profileImage;
                    previewContainer.style.display = 'flex';
                    placeholder.style.display = 'none';
                }
            } else {
                removeCollectionImage();
            }

            // Set selected books in Select2
            if (collection.books && collection.books.length > 0) {
                const bookIds = collection.books.map(b => b.id);
                $('#collectionBooks').val(bookIds).trigger('change');
            }

            // Set selected authors in Select2
            if (collection.authors && collection.authors.length > 0) {
                const authorIds = collection.authors.map(a => a.authorId);
                $('#collectionAuthors').val(authorIds).trigger('change');
            }

            // Set current collection ID for reordering
            currentCollectionId = collection.id;

            // Populate sortable books list
            populateSortableBooks(collection.books);

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
        const response = await fetch(`/collections/delete/${id}`, {
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

// ===== IMAGE HANDLING =====

let currentCollectionProfileImageBase64 = null;

function handleCollectionImageSelect(event) {
    const file = event.target.files[0];
    if (!file) return;

    // Validate file type
    const validTypes = ['image/jpeg', 'image/jpg', 'image/png'];
    if (!validTypes.includes(file.type)) {
        showMessage('collectionMessage', 'Please select a valid image file (JPG, JPEG, or PNG)', 'error');
        event.target.value = '';
        return;
    }

    // Validate file size (max 5MB)
    const maxSize = 5 * 1024 * 1024; // 5MB in bytes
    if (file.size > maxSize) {
        showMessage('collectionMessage', 'Image size must be less than 5MB', 'error');
        event.target.value = '';
        return;
    }

    // Read and convert to base64
    const reader = new FileReader();
    reader.onload = function(e) {
        const base64String = e.target.result;
        currentCollectionProfileImageBase64 = base64String;

        // Show preview
        const preview = document.getElementById('collectionImagePreview');
        const previewContainer = document.getElementById('collectionImagePreviewContainer');
        const placeholder = document.getElementById('collectionImageUploadPlaceholder');

        if (preview && previewContainer && placeholder) {
            preview.src = base64String;
            previewContainer.style.display = 'flex';
            placeholder.style.display = 'none';
        }
    };
    reader.readAsDataURL(file);
}

function removeCollectionImage() {
    currentCollectionProfileImageBase64 = null;

    const fileInput = document.getElementById('collectionProfileImage');
    const preview = document.getElementById('collectionImagePreview');
    const previewContainer = document.getElementById('collectionImagePreviewContainer');
    const placeholder = document.getElementById('collectionImageUploadPlaceholder');

    if (fileInput) fileInput.value = '';
    if (preview) preview.src = '';
    if (previewContainer) previewContainer.style.display = 'none';
    if (placeholder) placeholder.style.display = 'flex';
}

// ===== BOOK REORDERING =====

let sortableInstance = null;
let currentCollectionId = null;

function initializeSortable() {
    const sortableList = document.getElementById('sortableBooksList');
    if (!sortableList) return;

    // Destroy previous instance if exists
    if (sortableInstance) {
        sortableInstance.destroy();
    }

    // Create new Sortable instance
    sortableInstance = Sortable.create(sortableList, {
        animation: 150,
        ghostClass: 'sortable-ghost',
        chosenClass: 'sortable-chosen',
        handle: '.book-drag-handle',
        onEnd: function (evt) {
            // Update order numbers after drag
            updateOrderNumbers();
        }
    });
}

function updateOrderNumbers() {
    const items = document.querySelectorAll('.sortable-book-item');
    items.forEach((item, index) => {
        const orderNumberEl = item.querySelector('.book-order-number');
        if (orderNumberEl) {
            orderNumberEl.textContent = index + 1;
        }
        item.setAttribute('data-order', index);
    });
}

function populateSortableBooks(books) {
    const sortableList = document.getElementById('sortableBooksList');
    const reorderSection = document.getElementById('reorderBooksSection');

    if (!books || books.length === 0) {
        if (reorderSection) reorderSection.style.display = 'none';
        return;
    }

    if (reorderSection) reorderSection.style.display = 'block';

    // Sort books by displayOrder
    const sortedBooks = books.sort((a, b) => a.displayOrder - b.displayOrder);

    let html = '';
    sortedBooks.forEach((book, index) => {
        const statusClass = book.status.toLowerCase().replace(' ', '-');
        html += `
            <div class="sortable-book-item" data-book-id="${book.bookId}" data-order="${index}">
                <div class="book-order-number">${index + 1}</div>
                <div class="book-drag-handle">
                    <i class="fas fa-grip-vertical"></i>
                </div>
                <div class="book-info">
                    <p class="book-title-sort">${book.title}</p>
                    <p class="book-author-sort">${book.author}</p>
                </div>
                <span class="book-status-badge ${statusClass}">${book.status}</span>
            </div>
        `;
    });

    sortableList.innerHTML = html;
    initializeSortable();
}

async function saveBookOrder() {
    if (!currentCollectionId) {
        showMessage('collectionMessage', 'No collection selected', 'error');
        return;
    }

    const items = document.querySelectorAll('.sortable-book-item');
    const books = [];

    items.forEach((item, index) => {
        books.push({
            bookId: parseInt(item.getAttribute('data-book-id')),
            displayOrder: index
        });
    });

    try {
        const response = await fetch(`/collections/${currentCollectionId}/books/reorder`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ books: books })
        });

        if (response.ok) {
            showMessage('collectionMessage', 'Reading order updated successfully!', 'success');
            // Reload collections to reflect changes
            await loadCollections();
        } else {
            const errorMsg = await getErrorMessage(response);
            showMessage('collectionMessage', errorMsg, 'error');
        }
    } catch (error) {
        console.error('Error saving book order:', error);
        showMessage('collectionMessage', 'Failed to save reading order', 'error');
    }
}
