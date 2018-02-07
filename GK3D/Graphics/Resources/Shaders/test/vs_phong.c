#version 330

in vec3 vPosition;
in vec3 vColor;  
in vec3 vNormal;

out vec3 v_norm;
out vec3 v_pos;
out vec4 v_color;

uniform mat4 projection;
uniform mat4 model;
uniform mat4 view;

void main()
{
	gl_Position = projection * view * model * vec4(vPosition, 1.0);
	v_color = vec4( vColor, 1.0);

	mat3 normMatrix = transpose(inverse(mat3(model)));
	v_norm = normMatrix * vNormal;
	v_pos = (model * vec4(vPosition, 1.0)).xyz;
}