# Custom OIDC Single Sign-On (SSO)

A custom OpenID Connect (OIDC) based single sign-on implementation built with .NET MVC and Redis session management.

## Overview

This is a proof-of-concept (POC) implementation of a custom OIDC-compliant authentication server with MVC architecture. It handles user authentication, token generation, and secure session management using Redis.

## Technology Stack

- **.NET MVC** - Web application framework
- **OpenID Connect** - Authentication protocol implementation
- **Redis** - Distributed session management
- **PostgreSQL** - Data persistence
- **JWT** - Token generation with RSA encryption

## Key Features

- Custom OIDC authentication server
- MVC-based controller architecture
- Redis-based session management
- JWT token generation and validation
- User authentication and authorization flows

## OIDC Standards Implemented

This implementation follows the following OpenID Connect and OAuth 2.0 specifications:

- **OpenID Connect Core 1.0** - Core authentication protocol
- **OAuth 2.0 Authorization Framework** (RFC 6749) - Authorization protocol
- **JSON Web Token (JWT)** (RFC 7519) - Token format with RSA signatures
- **OpenID Connect Discovery 1.0** - Server metadata exposure via `.well-known/openid-configuration`
- **OpenID Connect Session Management** - User session handling
- **Authorization Code Flow** - Primary authentication flow implementation

### Implemented Endpoints

- `/.well-known/openid-configuration` - Server metadata and capability discovery
- `/authorize` - Authorization endpoint for user authentication
- `/token` - Token endpoint for authorization code exchange
- `/userinfo` - User info endpoint for retrieving user details

## Getting Started

1. Ensure Redis and PostgreSQL are running
2. Restore dependencies: `dotnet restore`
3. Configure connection strings in `appsettings.json`
4. Run: `dotnet run --project OIDC/OIDC.csproj`

## Project Structure

- **OIDC/** - Main MVC application with authentication controllers
- **Utility/** - Shared utility libraries and services
- **Protos/** - gRPC service definitions
