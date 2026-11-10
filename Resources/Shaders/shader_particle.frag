#version 330 core

out vec4 outColor;

struct Material
{
   sampler2D diffuse;
   sampler2D specular;
   float shininess;
   float smoothness;
};

struct AmbientLight
{
   vec3 color;
};

struct Light
{
   // Directional & Point & Spot.
   vec3 color;

   // Directional & Spot.
   vec3 direction;

   // Point & Spot.
   vec3 position;

   // Point & Spot.
   float range;

   // Spot.
   float cutoffInner;  // cos(radius)
   float cutoffOutter; // cos(radius)
};


vec3 CalculateDirectionalLight(Light light, vec3 normal, vec3 viewDir);

vec3 CalculatePointLight(Light light, vec3 normal, vec3 fragPos, vec3 viewDir);

vec3 CalculateSpotLight(Light light, vec3 normal, vec3 fragPos, vec3 viewDir);

in vec2 texCoord;
in vec3 normal;
in vec3 fragPosition;

uniform AmbientLight ambient;
uniform Material material;

uniform Light sunLight;
#define NR_LIGHTS 4
uniform Light lights[NR_LIGHTS];

void main()
{
   vec4 tex = texture(material.diffuse, texCoord);
   vec3 color = vec3(tex);
   float alpha = tex.x;

   if(alpha < 0.05)
      discard;

   outColor = vec4(color, alpha * material.smoothness); // * tex
}


vec3 CalculateDirectionalLight(Light light, vec3 normal, vec3 viewDir)
{
   vec3 dir = normalize(-light.direction);
   vec3 reflectDir = reflect(-dir, normal);

   vec3 diffuse = max(dot(normal, dir), 0.0) * light.color * vec3(texture(material.diffuse, texCoord));
   vec3 specular = material.smoothness * pow(max(dot(viewDir, reflectDir), 0.0), material.shininess) * light.color * vec3(texture(material.specular, texCoord));

   return diffuse + specular;
}

vec3 CalculatePointLight(Light light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
   float linear = 5.0 / light.range;
   float quadratic = 88.2 / (light.range * light.range);

   vec3 dir = normalize(light.position - fragPos);
   vec3 reflectDir = reflect(-dir, normal);

   vec3 diffuse = max(dot(normal, dir), 0.0) * light.color * vec3(texture(material.diffuse, texCoord));
   vec3 specular = material.smoothness * pow(max(dot(viewDir, reflectDir), 0.0), material.shininess) * light.color * vec3(texture(material.specular, texCoord));

   float dist = length(light.position - fragPos);
   float attenuation = 1.0 / (1.0 + linear * dist + quadratic * dist * dist);

   diffuse *= attenuation;
   specular *= attenuation;

   return diffuse + specular;
}

vec3 CalculateSpotLight(Light light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
   vec3 dir = normalize(light.position - fragPos);

   float theta = dot(dir, normalize(-light.direction));
   float eps = light.cutoffInner - light.cutoffOutter;

   if (theta > light.cutoffOutter)
   {
      float intensivity = clamp((theta - light.cutoffOutter) / eps, 0.0, 1.0);
      float linear = 5.0 / light.range;
      float quadratic = 88.2 / (light.range * light.range);

      vec3 reflectDir = reflect(-dir, normal);

      vec3 diffuse = max(dot(normal, dir), 0.0) * light.color * vec3(texture(material.diffuse, texCoord));
      vec3 specular = material.smoothness * pow(max(dot(viewDir, reflectDir), 0.0), material.shininess) * light.color * vec3(texture(material.specular, texCoord));

      float dist = length(light.position - fragPos);
      float attenuation = 1.0 / (1.0 + linear * dist + quadratic * dist * dist);

      diffuse *= attenuation * intensivity;
      specular *= attenuation * intensivity;

      return diffuse + specular;
   }

   return vec3(0, 0, 0);
}