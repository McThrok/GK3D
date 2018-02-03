#version 330

// Holds information about a light
struct Light {
 vec3 position;
 vec3 color;
 float ambientIntensity;
 float diffuseIntensity;
};

in vec3 v_norm;
in vec3 v_pos;
in vec4 color;
out vec4 outputColor;

uniform mat4 view;

// Material information
uniform vec3 material_ambient;
uniform vec3 material_diffuse;
uniform vec3 material_specular;
uniform float material_specExponent;

// Array of lights used in the shader
uniform Light light0;
uniform Light light1;
uniform Light light2;


vec4 getColor(Light light){
 outputColor = vec4(0,0,0,1);
 vec3 n = normalize(v_norm);
  
  vec3 lightvec = normalize(light0.position - v_pos);
  // Colors

  vec4 light_ambient = light0.ambientIntensity * vec4(light0.color, 0.0);
  vec4 light_diffuse = light0.diffuseIntensity * vec4(light0.color, 0.0);
  
  // Ambient lighting
  outputColor = outputColor + color * light_ambient * vec4(material_ambient, 0.0);

  // Diffuse lighting
  float lambertmaterial_diffuse = max(dot(n, lightvec), 0.0);

  // Spotlight, limit light to specific angle
  outputColor = outputColor + (light_diffuse * color * vec4(material_diffuse, 0.0)) * lambertmaterial_diffuse;

  // Specular lighting
  vec3 reflectionvec = normalize(reflect(-lightvec, v_norm));
  vec3 viewvec = normalize(vec3(inverse(view) * vec4(0,0,0,1)) - v_pos); 
  float material_specularreflection = max(dot(v_norm, lightvec), 0.0) * pow(max(dot(reflectionvec, viewvec), 0.0), material_specExponent);
  
  // Spotlight, specular reflections are also limited by angle
  outputColor = outputColor + vec4(material_specular * light0.color, 0.0) * material_specularreflection;

  return outputColor;
}

void main()
{

 outputColor = vec4(0,0,0,1);
 outputColor +=getColor(light0);
 outputColor +=getColor(light1);
 outputColor +=getColor(light2);
}