# Etapa de construcción
FROM node:slim as build
WORKDIR /app
COPY package*.json /app/
RUN npm install --force
COPY . /app
RUN npm run build

# Etapa final
FROM node:slim
WORKDIR /usr/app

# Crear un usuario no root (sintaxis para Debian)
RUN groupadd -r exampleusergroup && useradd -r -g exampleusergroup exampleuser

USER exampleuser

# Copiar archivos de compilación
COPY --from=build /app/dist/webflow/ ./dist/
COPY --from=build /app/package*.json ./ 
COPY --from=build /app/node_modules ./node_modules/

# Definir la variable de entorno para el puerto
ENV PORT=4000
EXPOSE 4000

# Comando de inicio para Angular SSR
CMD ["node", "dist/server/server.mjs"]


