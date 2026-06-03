# Caller and Callee roles align with TCP connection roles

WebRTC roles (Caller/Callee, defined by offer/answer semantics) will be bound to transport roles (TCP client/server) to avoid confusion in the BasicVideoChat example. The Caller initiates the TCP connection (acts as client) and sends the offer; the Callee listens for the TCP connection (acts as server) and responds with an answer.

We rejected decoupling transport and WebRTC roles because it adds cognitive load without benefit — a single peer cannot simultaneously listen (server) while also creating the offer (Caller) in a direct peer-to-peer TCP model. Binding them simplifies the example and documentation, making it clear to new readers that roles are stable and consistent throughout the session.
