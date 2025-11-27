# Gestor de Notas MongoDB

## Capturas de Pantalla

### Vista Principal
![Imagen de WhatsApp 2025-11-26 a las 18 43 20_86c95bf8](https://github.com/user-attachments/assets/72d84a8a-5dee-495e-bd8c-d160d8ad7c33)

### Crear/Editar Nota
![Imagen de WhatsApp 2025-11-26 a las 18 43 20_7b8b7630](https://github.com/user-attachments/assets/c19ba76e-14e9-44dd-a572-b7f957479146)
![Imagen de WhatsApp 2025-11-26 a las 18 43 19_398f21c1](https://github.com/user-attachments/assets/267874cc-9af8-4dae-8340-ceea982fb085)
![Imagen de WhatsApp 2025-11-26 a las 18 43 21_de448703](https://github.com/user-attachments/assets/83101bce-aac1-45c6-8e28-a312ff8c3b76)
![Imagen de WhatsApp 2025-11-26 a las 18 43 21_31d472e2](https://github.com/user-attachments/assets/f1eb63e1-bc7a-4f96-ba7d-67af61f9bf93)


### MongoDB
![Imagen de WhatsApp 2025-11-26 a las 18 43 22_b58cb42f](https://github.com/user-attachments/assets/7a1da4e5-dd67-4c1b-8ecb-513981f36576)

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
