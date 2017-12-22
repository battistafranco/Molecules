# Molecules

#Test realizado para la empresa Arcarys
#Contiene un alta de moléculas, que se consume a través de una WEB API dentro del mismo proyecto para simplificar tareas.
#Contiene un método de validación de valencias para los átomos seleccionados.
#Con motivos se simplificacion se creo solamente un sólo proyecto web con 2 controladores, un MVC controller para manejar las vistas y otro API controller para realizar las consultas a la base de datos.
#La base de datos es un archivo .TXT que se aloja en la carpeta App_Data, donde se guardar los registros de las móleculas ingresadas en formatos JSON.
#IMPORTANTE: para que la API funcione debe configurar el puerto utilizado por su servidor web local en el archivo Web.Config -> <add key="port" value="16302" />, debe cambiar el value por el valor correspondiente.