
# Study Hub

Real time study room server with built in OpenAI agent. The server is built using .Net 9, Microsoft Idenity, and SignalR. Join rooms with classmates to chat about upcoming assignments. Each student can ask the AI agent for help explaining a problem.


## Why?

I created this application to gain a deeper understanding of several key .Net 9 features and increase my portfolio status.
## Run Locally

Requirements
- dotnet-sdk >= 9.0

Run 
```
touch .env
# add your personal "OpenAiKey" key to .env file
dotnet build
dotnet run
```


## Auth Endpoints

```
/login
/register
```

## Room Endpoints
```
/api/create-room
/api/delete-room/{roomName}
```

## Chat Room
```
/chat
```
#### SignalR Methods
- JoinRoomAsync
- SendMessageAsync
- LeaveRoomAsync
- GetAiMessageAsync
- SendAiRequestAsync
## Key .Net Skills

- .Net 9
- Controller based API
- Repository Pattern
- Interface Pattern
- Dependency Injection
- Entity Framework Core
- Identity JWT authentication and authorization
- SignalR Hubs
- Real-time communication
## Features

- JWT Auth with .Net Identity
- Real-time chat using SignalR
- Ability for users to create, join, and delete study rooms
- Rate limit requests to AI agent
- Dockerization


