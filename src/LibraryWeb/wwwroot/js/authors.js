// ===== AUTHORS PAGE SCRIPT =====

document.addEventListener('DOMContentLoaded', function () {
    // Load saved theme or default to light
    const savedTheme = localStorage.getItem('theme') || 'light';
    applyTheme(savedTheme);

    loadAuthors();
    initializeAuthorForm();
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

function initializeAuthorForm() {
    const form = document.getElementById('formAuthor');
    if (form) {
        form.addEventListener('submit', async function(e) {
            e.preventDefault();

            const authorId = document.getElementById('editAuthorId').value;
            const authorData = {
                name: document.getElementById('authorName').value,
                nationality: document.getElementById('authorNationality').value || null,
                bio: document.getElementById('authorBio').value || null
            };

            if (authorId) {
                await updateAuthor(authorId, authorData);
            } else {
                await addAuthor(authorData);
            }
        });
    }
}

function showAuthorForm() {
    const formSection = document.getElementById('authorFormSection');
    const tableSection = document.getElementById('authorsSection');

    if (formSection && tableSection) {
        formSection.classList.remove('hidden');
        tableSection.classList.add('hidden');
    }
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
        const response = await fetch('/authors/getall');
        if (response.ok) {
            const authors = await response.json();
            displayAuthors(authors);
        } else {
            console.error('Failed to load authors:', response.status, response.statusText);
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
        const response = await fetch('/authors/create', {
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
        const response = await fetch(`/authors/update/${id}`, {
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
        const response = await fetch(`/authors/get/${id}`);

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
        const response = await fetch(`/authors/delete/${id}`, {
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
        showMessage('authorMessage', 'Error deleting author', 'error');
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
