# PRUEBA TÉCNICA PRÁCTICA en .NET

# **Documentación de la API**

Esta documentación describe el funcionamiento de una API para gestionar información de personas en un proyecto. La API proporciona funcionamiento básico, utiliza Swagger para poder hacer peticiones, el framework ASP.NET.8 Core y está implementado en el lenguaje C#. 

## **Descripción general**

La API proporciona endpoints para realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre entidades de personas. Las operaciones disponibles son:

- Obtener todas las personas.
- Obtener una persona por su ID.
- Crear una nueva persona.
- Actualizar los datos de una persona.
- Eliminar una persona.

## **Configuración del controlador**

El controlador **`ProyectController`** gestiona las solicitudes HTTP relacionadas con las personas. Este controlador está ubicado en el espacio de nombres **`Proyecto_Api.Controllers`** y se encuentra en el archivo **`ProyectController.cs`**.

### **Dependencias**

El controlador depende de los siguientes servicios:

- **`ILogger<ProyectController>`**: Para registrar eventos y mensajes de registro.
- **`IPersonaRepositorie`**: Para acceder a los datos de las personas en el repositorio.
- **`IMapper`**: Para mapear entre modelos de datos y DTOs (objetos de transferencia de datos).

## **Endpoints**

### **Obtener todas las personas**

- Método HTTP: **`GET`**
- Ruta: **`/api/Proyect`**
- Descripción: Obtiene todas las personas almacenadas en el sistema.
- Respuestas:
    - **`200 OK`**: Se devuelve la lista de personas.
    - **`500 Internal Server Error`**: Error interno del servidor.

### **Obtener una persona por su ID**

- Método HTTP: **`GET`**
- Ruta: **`/api/Proyect/{id}`**
- Descripción: Obtiene los datos de una persona según su ID.
- Parámetros de ruta:
    - **`id`** (integer): El ID de la persona que se desea obtener.
- Respuestas:
    - **`200 OK`**: Se devuelve la persona encontrada.
    - **`404 Not Found`**: La persona no fue encontrada.
    - **`400 Bad Request`**: La solicitud es incorrecta.

### **Crear una nueva persona**

- Método HTTP: **`POST`**
- Ruta: **`/api/Proyect`**
- Descripción: Crea una nueva persona en el sistema.
- Parámetros de cuerpo (Body):
    - **`createPersonaDto`**: Datos de la persona a crear.
- Respuestas:
    - **`201 Created`**: La persona fue creada exitosamente.
    - **`400 Bad Request`**: La solicitud es incorrecta o los datos de la persona son inválidos.
    - **`500 Internal Server Error`**: Error interno del servidor.

### **Actualizar los datos de una persona**

- Método HTTP: **`PUT`**
- Ruta: **`/api/Proyect/{id}`**
- Descripción: Actualiza los datos de una persona existente en el sistema.
- Parámetros de ruta:
    - **`id`** (integer): El ID de la persona que se desea actualizar.
- Parámetros de cuerpo (Body):
    - **`updatePersonaDto`**: Nuevos datos de la persona.
- Respuestas:
    - **`204 No Content`**: La actualización se realizó con éxito.
    - **`400 Bad Request`**: La solicitud es incorrecta o los datos de la persona son inválidos.

### **Eliminar una persona**

- Método HTTP: **`DELETE`**
- Ruta: **`/api/Proyect/{id}`**
- Descripción: Elimina una persona del sistema según su ID.
- Parámetros de ruta:
    - **`id`** (integer): El ID de la persona que se desea eliminar.
- Respuestas:
    - **`204 No Content`**: La persona fue eliminada correctamente.
    - **`404 Not Found`**: La persona no fue encontrada.
    - **`400 Bad Request`**: La solicitud es incorrecta.

## **Conclusiones**

La API proporciona una interfaz simple y efectiva para gestionar información de personas en un proyecto. Los endpoints permiten realizar operaciones CRUD de manera segura y eficiente. Se recomienda utilizar esta documentación como referencia al interactuar con la API.

## **Proximas Actualizaciones**

-**Vistas**
-**Seguridad**

