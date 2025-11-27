# Gestor de Notas MongoDB

## Capturas de Pantalla

### Vista Principal
<img width="1275" height="781" alt="image" src="https://github.com/user-attachments/assets/bfb10dc7-b90e-47b2-b1ac-4516e75891e6" />

### Crear/Editar Nota
<img width="1279" height="775" alt="image" src="https://github.com/user-attachments/assets/f61ede7b-4275-41fa-8c70-77984d0d321a" />
<img width="1276" height="783" alt="image" src="https://github.com/user-attachments/assets/5fae1ec3-d56a-4142-af15-dd6832d903cd" />
<img width="1276" height="786" alt="image" src="https://github.com/user-attachments/assets/c392a945-2a17-4d1f-923a-255788f36971" />
<img width="1273" height="779" alt="image" src="https://github.com/user-attachments/assets/e4a957a2-b2e8-4fa0-b42c-b1392d6806b7" />

### MongoDB
<img width="1646" height="758" alt="image" src="https://github.com/user-attachments/assets/8e45711e-052f-424f-86e6-3ba7db684439" />

## Instalación

### 1️- Clonar el Repositorio

```bash
git clone https://github.com/EduardoHas671/gestor-mongo-db.git
cd GestorNotas_MongoDB
```

### 2- Configurar MongoDB

1. Crea una cuenta gratuita en https://cloud.mongodb.com/
2. Crea un nuevo cluster (selecciona el tier gratuito M0)
3. Crea un usuario de base de datos:
   - Ve a "Database Access"
   - Click "Add New Database User"
   - Guarda usuario y contraseña
4. Conectar a C#:
   - Ve a "Database"
   - Ve a "Clusters"
   - Ahí aparecera tu base de datos
     
5. Obtén tu cadena de conexión:
   - Selecciona "Connect"
   - Selecciona "MongoDB for VS code"
   - Copia la connection string que se te asigne
   - Cambia el usuario y la contraseña por la que creaste
   - pega el link de la conexión en el json en esta sección
   "ConnectionString": "AQUI PON TU STRING CONNECTION",

### 3️- Configurar la Aplicación

#### Crear Archivo de JSON en ejectuble

1. Abre la carpeta en donde aparecen los paquetes de nuget y el .exe del form
2. Agrega ahí un archivo nombrado como "appsettings.json" 
3. Escribe en el archivo el json que esta a continuación pero reemplazando la ConnectionString por la tuya 

**Para MongoDB:**
```json
{
 "MongoDB": {
  "ConnectionString": "mongodb+srv://tuusuario:tupassword@cluster0.xxxxx.mongodb.net/",
  "DatabaseName": "GestorNotas",
  "CollectionName": "Notas"
  }
}
```

> **Reemplaza `tuusuario`, `tupassword` y `cluster0.xxxxx` con tus credenciales reales de MongoDB Atlas**
