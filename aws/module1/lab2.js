// 🔹 Ejercicio 2: Crear tu primera función Lambda

// En la consola, busca “Lambda”.

// Clic en Create function → Author from scratch.

// Nombre: HelloLambda

// Runtime: Node.js 18.x (puede ser Python o C# si prefieres).

// Permisos: crea un nuevo rol con permisos básicos de Lambda.

// Código:

exports.handler = async (event) => {
    return {
        statusCode: 200,
        body: JSON.stringify({ message: "Hello from AWS Lambda!" }),
    };
};

// aws lambda create-function \
//   --function-name HelloLambda \
//   --runtime nodejs18.x \
//   --role arn:aws:iam::<tu-cuenta>:role/<tu-rol-lambda> \
//   --handler index.handler \
//   --zip-file fileb://function.zip