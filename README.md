### **Products API**
This is a simple RESTful API that allows users to manage a collection of products. It provides several endpoints to create, retrieve, and filter products. The API is secured, meaning that certain operations (such as creating a product or retrieving all products) require authentication.

The API is secured with JWT-based authentication. Users must provide a valid token to access protected endpoints, ensuring that only authorised individuals can perform certain actions like creating or viewing products.

In Development mode it uses a Fake Authorisation Handler to make it easer to test it via Swagger and also to perform integration tests.

The implementation uses MediatR because it provides several benefits such as separation of concerns (each handler is responsible for a use case), improved testeability (handlers don't usually depend upon HTTP context), extensibility (thanks to pipelines like the one we are using for validation in this API).

Below is a simple architecture diagram illustrating how the Products service can be part of a distributed or microservices event-driven architecture. This architecture includes other components such as Orders, Payments, and a Message Broker for communication.

### **Architecture Diagram: Event-Driven Microservices Architecture**

```plaintext
+------------------------------------+
|                                    |
|       API Gateway / Load Balancer  |
|                                    |
+------------------+-----------------+
                   |
                   |
+------------------v-----------------+
|                                    |
|       Products Service (API)       |
|                                    |
+------------------+-----------------+
                   |
                   |
            +------v------+
            |             |
            | Message Broker (e.g., |
            | Kafka/RabbitMQ)       |
            |             |
            +------^------+
                   |
                   |
+------------------+-----------------+      +------------------+------------------+      +------------------+------------------+
|                                    |      |                                    |      |                                    |
|        Orders Service              |      |        Payments Service            |      |        Notifications Service       |
|                                    |      |                                    |      |                                    |
+------------------------------------+      +------------------------------------+      +------------------------------------+

```

### **Components Overview:**

1. **API Gateway / Load Balancer**:
   - **Function**: Acts as an entry point for client requests. Routes requests to the appropriate service based on the endpoint.
   - **Responsibilities**: Authentication, rate limiting, routing, and load balancing.

2. **Products Service**:
   - **Function**: Manages product-related data such as creation, retrieval, and filtering of products.
   - **Responsibilities**: Provides RESTful endpoints for product operations. Sends events (e.g., "ProductCreated") to the message broker when products are created or updated.

3. **Message Broker (e.g., Kafka, RabbitMQ)**:
   - **Function**: Facilitates communication between microservices in an event-driven manner.
   - **Responsibilities**: Publishes and subscribes to events such as "ProductCreated," "OrderPlaced," and "PaymentProcessed." Ensures loose coupling between services.

4. **Orders Service**:
   - **Function**: Handles order creation and management.
   - **Responsibilities**: Consumes product-related events to validate product availability. Publishes events like "OrderPlaced" after an order is successfully created.

5. **Payments Service**:
   - **Function**: Manages payment processing for orders.
   - **Responsibilities**: Consumes "OrderPlaced" events to initiate payments. Publishes "PaymentProcessed" events upon successful payment.

6. **Notifications Service**:
   - **Function**: Sends notifications to users based on various events.
   - **Responsibilities**: Listens to events like "OrderPlaced," "PaymentProcessed," and "ProductCreated" to notify customers via email, SMS, or push notifications.

### **Flow Example:**

1. **Product Creation**:
   - A new product is created via the Products Service API.
   - The Products Service emits a "ProductCreated" event to the message broker.

2. **Order Placement**:
   - The Orders Service consumes the "ProductCreated" event to ensure the product is available.
   - When a customer places an order, the Orders Service emits an "OrderPlaced" event to the message broker.

3. **Payment Processing**:
   - The Payments Service listens to the "OrderPlaced" event, processes the payment, and emits a "PaymentProcessed" event.

4. **Notification**:
   - The Notifications Service consumes relevant events like "OrderPlaced" and "PaymentProcessed" to notify the customer about the order and payment status.
