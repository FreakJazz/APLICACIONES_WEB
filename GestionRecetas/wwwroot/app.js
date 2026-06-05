let recetas = [];
let ingredientes = [];

async function cargarRecetas() {
    try {
        const res = await fetch('/api/recetas');
        if (res.ok) {
            recetas = await res.json();
            mostrarRecetas();
        }
    } catch (error) {
        console.error('Error al cargar recetas');
    }
}

async function cargarIngredientes() {
    try {
        const res = await fetch('/api/ingredientes');
        if (res.ok) {
            ingredientes = await res.json();
            mostrarIngredientes();
        }
    } catch (error) {
        console.error('Error al cargar ingredientes');
    }
}

function mostrarRecetas() {
    const html = recetas.map(r => `
        <div class="col-md-4 mb-3">
            <div class="card h-100 border-danger">
                <div class="card-body">
                    <h5 class="card-title text-danger">${r.nombre}</h5>
                    <p class="card-text">${r.descripcion || 'Sin descripción'}</p>
                    <small class="text-muted">⏱️ ${r.tiempoPreparacion} min | 📊 ${r.dificultad}</small>
                </div>
                <div class="card-footer bg-light d-flex gap-2">
                    <button class="btn btn-sm btn-warning" onclick="editarReceta(${r.recetaId}, '${r.nombre.replace(/'/g, "\\'")}', '${(r.descripcion || '').replace(/'/g, "\\'")}', ${r.tiempoPreparacion}, '${r.dificultad}')">Editar</button>
                    <button class="btn btn-sm btn-danger" onclick="eliminarReceta(${r.recetaId})">Eliminar</button>
                </div>
            </div>
        </div>
    `).join('');
    
    document.getElementById('listRecetas').innerHTML = html || '<p>Sin recetas</p>';
}

function mostrarIngredientes() {
    const html = ingredientes.map(i => `
        <tr>
            <td>${i.nombre}</td>
            <td>${i.cantidad}</td>
            <td>${i.unidad}</td>
            <td>${i.calorias || '-'}</td>
            <td>
                <button class="btn btn-sm btn-warning" onclick="editarIngrediente(${i.ingredienteId}, '${i.nombre.replace(/'/g, "\\'")}', ${i.cantidad}, '${i.unidad}', ${i.calorias || 'null'})">Editar</button>
                <button class="btn btn-sm btn-danger" onclick="eliminarIngrediente(${i.ingredienteId})">Eliminar</button>
            </td>
        </tr>
    `).join('');
    
    document.getElementById('tbodyIngredientes').innerHTML = html || '<tr><td colspan="5">Sin ingredientes</td></tr>';
}

document.getElementById('formReceta').addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const btn = e.target.querySelector('button');
    const isEdit = btn.dataset.mode === 'edit';
    const recetaId = btn.dataset.id;
    
    const receta = {
        nombre: document.getElementById('nombre').value,
        descripcion: document.getElementById('descripcion').value,
        tiempoPreparacion: parseInt(document.getElementById('tiempo').value),
        dificultad: document.getElementById('dificultad').value
    };
    
    if (isEdit) {
        receta.recetaId = parseInt(recetaId);
    }
    
    try {
        const url = isEdit ? `/api/recetas/${recetaId}` : '/api/recetas';
        const method = isEdit ? 'PUT' : 'POST';
        
        const res = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(receta)
        });
        
        if (res.ok) {
            alert(isEdit ? '✓ Receta actualizada' : '✓ Receta creada');
            document.getElementById('formReceta').reset();
            btn.textContent = 'Crear Receta';
            btn.dataset.mode = '';
            btn.dataset.id = '';
            cargarRecetas();
        } else {
            alert('Error');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error');
    }
});

document.getElementById('formIngrediente').addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const btn = e.target.querySelector('button');
    const isEdit = btn.dataset.mode === 'edit';
    const ingredienteId = btn.dataset.id;
    
    const ingrediente = {
        nombre: document.getElementById('nomIng').value,
        cantidad: parseFloat(document.getElementById('cantIng').value),
        unidad: document.getElementById('unidadIng').value,
        calorias: document.getElementById('calIng').value ? parseInt(document.getElementById('calIng').value) : null
    };
    
    if (isEdit) {
        ingrediente.ingredienteId = parseInt(ingredienteId);
    }
    
    try {
        const url = isEdit ? `/api/ingredientes/${ingredienteId}` : '/api/ingredientes';
        const method = isEdit ? 'PUT' : 'POST';
        
        const res = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(ingrediente)
        });
        
        if (res.ok) {
            alert(isEdit ? '✓ Ingrediente actualizado' : '✓ Ingrediente creado');
            document.getElementById('formIngrediente').reset();
            btn.textContent = 'Crear Ingrediente';
            btn.dataset.mode = '';
            btn.dataset.id = '';
            cargarIngredientes();
        } else {
            alert('Error');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error');
    }
});

async function eliminarReceta(id) {
    if (!confirm('¿Eliminar receta?')) return;
    
    try {
        const res = await fetch(`/api/recetas/${id}`, { method: 'DELETE' });
        if (res.ok) {
            alert('✓ Receta eliminada');
            cargarRecetas();
        } else {
            alert('Error al eliminar');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

async function eliminarIngrediente(id) {
    if (!confirm('¿Eliminar ingrediente?')) return;
    
    try {
        const res = await fetch(`/api/ingredientes/${id}`, { method: 'DELETE' });
        if (res.ok) {
            alert('✓ Ingrediente eliminado');
            cargarIngredientes();
        } else {
            alert('Error al eliminar');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

function editarReceta(id, nombre, descripcion, tiempo, dificultad) {
    document.getElementById('nombre').value = nombre;
    document.getElementById('descripcion').value = descripcion;
    document.getElementById('tiempo').value = tiempo;
    document.getElementById('dificultad').value = dificultad;
    
    const btn = document.querySelector('#formReceta button');
    btn.textContent = 'Actualizar Receta';
    btn.dataset.id = id;
    btn.dataset.mode = 'edit';
    
    document.querySelector('a[href="#crearReceta"]').click();
}

function editarIngrediente(id, nombre, cantidad, unidad, calorias) {
    document.getElementById('nomIng').value = nombre;
    document.getElementById('cantIng').value = cantidad;
    document.getElementById('unidadIng').value = unidad;
    document.getElementById('calIng').value = calorias || '';
    
    const btn = document.querySelector('#formIngrediente button');
    btn.textContent = 'Actualizar Ingrediente';
    btn.dataset.id = id;
    btn.dataset.mode = 'edit';
    
    document.querySelector('a[href="#crearIngrediente"]').click();
}

cargarRecetas();
cargarIngredientes();
