#version 330 core
layout(location = 0) in vec3 Position;
layout(location = 1) in vec2 TexCoord;
layout(location = 2) in vec3 Normal;

out vec2 texCoord;
out vec3 normal;
out vec3 fragPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat3 normalMatrix;

#define MAX_INSTANCES 100

uniform vec3 offsets[MAX_INSTANCES];


void main()
{
   vec3 offset = offsets[gl_InstanceID];

   texCoord = TexCoord;
   normal = normalMatrix * Normal;
   fragPosition = vec3(view * model * vec4(Position + offset, 1.0));

   gl_Position = projection * view * model * vec4(Position + offset, 1.0);
}